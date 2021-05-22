namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Common.Enums;
    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels.Administration.Products;
    using Microsoft.EntityFrameworkCore;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IDeletableEntityRepository<ProductCategory> productCategoryRepository;

        public ProductsService(
            IDeletableEntityRepository<Product> productsRepository,
            IDeletableEntityRepository<ProductCategory> productCategoryRepository)
        {
            this.productsRepository = productsRepository;
            this.productCategoryRepository = productCategoryRepository;
        }

        public async Task<IEnumerable<T>> GetAllAdminAsync<T>()
        {
            var products = await this.productsRepository
                .AllWithDeleted()
                .OrderBy(p => p.Name)
                .To<T>()
                .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(int take, int skip, SortBy sortBy = SortBy.MostPopular, int? minPrice = null, int? maxPrice = null)
        {
            var products = this.productsRepository
                .All();

            if (minPrice != null && maxPrice != null)
            {
                products = products
                    .Where(p => p.Price >= minPrice.Value
                                && p.Price <= maxPrice.Value);
            }

            products = sortBy switch
            {
                SortBy.Newest => products.OrderByDescending(p => p.CreatedOn),
                SortBy.PriceDescending => products.OrderByDescending(p => p.Price),
                SortBy.PriceAscending => products.OrderBy(p => p.Price),
                _ => products.OrderByDescending(p => p.OrderProducts.Count),
            };

            return await products
                .Skip(skip)
                .Take(take)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByCategoryNameAsync<T>(string name, int take, int skip, SortBy sortBy = SortBy.MostPopular, int? minPrice = null, int? maxPrice = null)
        {
            var products = this.productsRepository
                .All()
                .Where(p => p.ProductCategories
                    .Any(pc => pc.Category.Name == name
                               && pc.ProductId == p.Id));

            if (minPrice != null && maxPrice != null)
            {
                products = products
                    .Where(p => p.Price >= minPrice.Value
                                && p.Price <= maxPrice.Value);
            }

            products = sortBy switch
            {
                SortBy.Newest => products.OrderByDescending(p => p.CreatedOn),
                SortBy.PriceDescending => products.OrderByDescending(p => p.Price),
                SortBy.PriceAscending => products.OrderBy(p => p.Price),
                _ => products.OrderByDescending(p => p.OrderProducts.Count),
            };

            return await products
                .Skip(skip)
                .Take(take)
                .To<T>()
                .ToListAsync();
        }

        public async Task<int> GetCountByCategoryIdAsync(int categoryId, int? minPrice = null, int? maxPrice = null)
        {
            var query = this.productsRepository
                .All()
                .Where(p => p.ProductCategories
                    .Any(pc => pc.Category.Id == categoryId
                               && pc.ProductId == p.Id));

            if (minPrice != null && maxPrice != null)
            {
                query = query
                    .Where(p => p.Price >= minPrice.Value
                                         && p.Price <= maxPrice.Value);
            }

            return await query.CountAsync();
        }

        public async Task<int> GetCountAsync(int? minPrice = null, int? maxPrice = null)
        {
            var query = this.productsRepository
                .All();

            if (minPrice != null && maxPrice != null)
            {
                query = query
                    .Where(p => p.Price >= minPrice.Value
                                && p.Price <= maxPrice.Value);
            }

            return await query.CountAsync();
        }

        public async Task<T> GetByIdAsync<T>(int? id)
        {
            var product = await this.productsRepository
                .All()
                .Where(p => p.Id == id.Value)
                .To<T>()
                .FirstOrDefaultAsync();

            if (product == null)
            {
                // TODO add message
                throw new NullReferenceException();
            }

            return product;
        }

        public async Task<T> GetByNameAsync<T>(string title)
        {
            var product = await this.productsRepository
                .All()
                .Where(p => p.Title == title.Replace('-', ' '))
                .To<T>()
                .FirstOrDefaultAsync();

            if (product == null)
            {
                // TODO add message
                throw new NullReferenceException();
            }

            return product;
        }

        public async Task<T> AdminGetByIdAsync<T>(int? id)
        {
            var product = await this.productsRepository
                .AllWithDeleted()
                .Where(p => p.Id == id.Value)
                .To<T>()
                .FirstOrDefaultAsync();

            if (product == null)
            {
                // TODO add message
                throw new NullReferenceException();
            }

            return product;
        }

        public async Task<int> DeleteRecoverAsync(int id)
        {
            var product = await this.productsRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                // TODO add message
                throw new NullReferenceException();
            }

            product.IsDeleted = !product.IsDeleted;
            await this.productsRepository.SaveChangesAsync();

            return id;
        }

        public async Task EditAsync(AdminProductEditViewModel viewModel)
        {
            var product = await this.productsRepository
                .AllWithDeleted()
                .Include(p => p.Images)
                .FirstOrDefaultAsync(c => c.Id == viewModel.Id);

            if (product == null)
            {
                // TODO add message
                throw new NullReferenceException();
            }

            var editedImages = viewModel
                .Images
                .Where(i => i.ImgUrl != null)
                .Select(i => new Image()
                {
                    CreatedOn = DateTime.UtcNow,
                    ImgUrl = i.ImgUrl,
                    ProductId = product.Id,
                }).ToList();

            var editedProductCategory = new ProductCategory()
            {
                CategoryId = int.Parse(viewModel.Categories[0].Name),
                ProductId = product.Id,
            };

            product.Name = viewModel.Name;
            product.Title = viewModel.Title;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;
            product.Quantity = viewModel.Quantity;
            product.Height = viewModel.Height;
            product.Width = viewModel.Width;
            product.Images = editedImages;

            var productCategory = await this.productCategoryRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(pc => pc.ProductId == product.Id);

            this.productCategoryRepository.HardDelete(productCategory);
            await this.productCategoryRepository.AddAsync(editedProductCategory);

            await this.productCategoryRepository.SaveChangesAsync();
            await this.productsRepository.SaveChangesAsync();
        }

        public async Task CreateAsync(AdminProductInputModel inputModel)
        {
            if (await this.productsRepository
                .AllWithDeleted().AnyAsync(p => p.Name == inputModel.Name))
            {
                throw new AmbiguousImplementationException(
                    string.Format(GlobalConstants.Product.OnCreateProductNotUniqueError, inputModel.Name));
            }

            var product = new Product()
            {
                CreatedOn = DateTime.UtcNow,
                Name = inputModel.Name,
                Title = inputModel.Title,
                Description = inputModel.Description,
                Price = inputModel.Price,
                Quantity = inputModel.Quantity,
                Height = inputModel.Height,
                Width = inputModel.Width,
            };

            var images = inputModel
                .Images
                .Where(i => i.ImgUrl != null)
                .Select(i => new Image()
                {
                    CreatedOn = DateTime.UtcNow,
                    ImgUrl = i.ImgUrl,
                    ProductId = product.Id,
                })
                .ToList();

            var productCategory = inputModel
                .Categories
                .Select(pc => new ProductCategory()
                {
                    ProductId = product.Id,
                    CategoryId = int.Parse(inputModel.Categories[0].Name),
                })
                .ToList();

            product.Images = images;
            product.ProductCategories = productCategory;

            await this.productsRepository.AddAsync(product);
            await this.productsRepository.SaveChangesAsync();
        }

        public async Task<decimal> MostExpensiveProductPrice()
        {
            return await this.productsRepository.All()
                .MaxAsync(p => p.Price);
        }

        public async Task<decimal> LeastExpensiveProductPrice()
        {
            return await this.productsRepository.All()
                .MinAsync(p => p.Price);
        }
    }
}
