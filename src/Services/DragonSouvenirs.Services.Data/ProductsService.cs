namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using DragonSouvenirs.Common;
    using DragonSouvenirs.Common.Enums;
    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Data.Common;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels.Administration.Products;
    using Microsoft.EntityFrameworkCore;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly IDeletableEntityRepository<ProductCategory> productCategoryRepository;
        private readonly IDeletableEntityRepository<FavouriteProduct> favouriteProductRepository;
        private readonly ICategoriesService categoriesService;
        private readonly Cloudinary cloudinary;

        public ProductsService(
            IDeletableEntityRepository<Product> productsRepository,
            IDeletableEntityRepository<ProductCategory> productCategoryRepository,
            IDeletableEntityRepository<FavouriteProduct> favouriteProductRepository,
            ICategoriesService categoriesService,
            Cloudinary cloudinary)
        {
            this.productsRepository = productsRepository;
            this.productCategoryRepository = productCategoryRepository;
            this.favouriteProductRepository = favouriteProductRepository;
            this.categoriesService = categoriesService;
            this.cloudinary = cloudinary;
        }

        // Get all WITH DELETED products, ordered by Name
        public async Task<IEnumerable<T>> GetAllAdminAsync<T>()
        {
            var products = await this.productsRepository
                .All()
                .OrderBy(p => p.Name)
                .To<T>()
                .ToListAsync();

            return products;
        }

        // Get DELETED products, ordered by Name
        public async Task<IEnumerable<T>> GetDeletedAsync<T>()
        {
            var products = await this.productsRepository
                .AllWithDeleted()
                .Where(p => p.IsDeleted)
                .OrderBy(p => p.Name)
                .To<T>()
                .ToListAsync();

            return products;
        }

        // Get All products
        // -> take, skip (pagination)
        // -> [OPTIONAL] sortBy, minPrice, maxPrice (custom sorting)
        // -> [OPTIONAL] searchString (searching)
        public async Task<IEnumerable<T>> GetAllAsync<T>(int take, int skip, SortBy sortBy = SortBy.MostPopular, int? minPrice = null, int? maxPrice = null, string searchString = null)
        {
            var products = this.productsRepository
                .All();

            products = FilterByPrice(minPrice, maxPrice, products);
            products = SortProducts(sortBy, products);

            return await products
                .Skip(skip)
                .Take(take)
                .To<T>()
                .ToListAsync();
        }

        // Get All products by category name
        // -> name (category name)
        // -> take, skip (pagination)
        // -> [OPTIONAL] sortBy, minPrice, maxPrice (custom sorting)
        // -> [OPTIONAL] searchString (searching)
        public async Task<IEnumerable<T>> GetAllByCategoryNameAsync<T>(string name, int take, int skip, SortBy sortBy = SortBy.MostPopular, int? minPrice = null, int? maxPrice = null)
        {
            var products = this.productsRepository
                .All()
                .Where(p => p.ProductCategories
                    .Any(pc => pc.Category.Name == name
                               && pc.ProductId == p.Id));

            products = FilterByPrice(minPrice, maxPrice, products);
            products = SortProducts(sortBy, products);

            return await products
                .Skip(skip)
                .Take(take)
                .To<T>()
                .ToListAsync();
        }

        // Get the count of products in a category
        // -> [OPTIONAL] minPrice, maxPrice (custom sorting)
        public async Task<int> GetCountByCategoryIdAsync(int categoryId, int? minPrice = null, int? maxPrice = null)
        {
            var products = this.productsRepository
                .All()
                .Where(p => p.ProductCategories
                    .Any(pc => pc.Category.Id == categoryId
                               && pc.ProductId == p.Id));

            products = FilterByPrice(minPrice, maxPrice, products);

            return await products.CountAsync();
        }

        // Get the top discounted products.
        public async Task<IEnumerable<T>> GetTopDiscountedItems<T>(int take = 8)
        {
            var products = this.productsRepository
                .All()
                .Where(p => p.DiscountPrice != null && p.Quantity > 0)
                .OrderByDescending(p => p.Price - p.DiscountPrice.Value)
                .ThenByDescending(p => p.OrderProducts.Count)
                .Take(take);

            return await products.To<T>().ToListAsync();
        }

        // Get the user's favourite products
        public async Task<IEnumerable<T>> GetFavouriteProductsAsync<T>(string userId)
        {
            var products = this.favouriteProductRepository
                .All()
                .Where(fp => fp.UserId == userId && fp.Product.IsDeleted == false)
                .OrderByDescending(fp => fp.CreatedOn);

            return await products.To<T>().ToListAsync();
        }

        // Add or Remove a product to the user's favourite products
        public async Task<bool> FavouriteProductAsync(string userId, string name)
        {
            // Get the product
            var product = await this.productsRepository
                .All()
                .FirstOrDefaultAsync(p => p.Name == name.Replace('-', ' '));

            if (product == null)
            {
                return false;
            }

            var favouriteProduct = await this.favouriteProductRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(fp => fp.UserId == userId
                                           && fp.ProductId == product.Id);

            // Check if product is already in user's favourite products
            if (favouriteProduct == null)
            {
                favouriteProduct = new FavouriteProduct()
                {
                    ProductId = product.Id,
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow,
                };
                await this.favouriteProductRepository.AddAsync(favouriteProduct);
            }

            // Remove the product if it's already added
            else
            {
                favouriteProduct.IsDeleted = favouriteProduct.IsDeleted != true;
            }

            await this.favouriteProductRepository.SaveChangesAsync();
            return true;
        }

        // Get the count of products
        // -> [OPTIONAL] minPrice, maxPrice (custom sorting)
        public async Task<int> GetCountAsync(int? minPrice = null, int? maxPrice = null)
        {
            var products = this.productsRepository
                .All();
            products = FilterByPrice(minPrice, maxPrice, products);

            return await products.CountAsync();
        }

        // Get product by Id
        public async Task<T> GetByIdAsync<T>(int? id)
        {
            var product = await this.productsRepository
                .All()
                .Where(p => p.Id == id.Value)
                .To<T>()
                .FirstOrDefaultAsync();

            return product;
        }

        // Get product by Title
        public async Task<T> GetByNameAsync<T>(string name)
        {
            var product = await this.productsRepository
                .All()
                .Where(p => p.Name == name.Replace('-', ' '))
                .To<T>()
                .FirstOrDefaultAsync();

            return product;
        }

        // Get WITH DELETED product by Id
        public async Task<T> AdminGetByIdAsync<T>(int? id)
        {
            var product = await this.productsRepository
                .AllWithDeleted()
                .Where(p => p.Id == id.Value)
                .To<T>()
                .FirstOrDefaultAsync();

            return product;
        }

        // [Soft Delete] Delete or Recover a product by Id
        public async Task<string> DeleteRecoverAsync(int id)
        {
            var product = await this.productsRepository
                .AllWithDeleted()
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return null;
            }

            product.IsDeleted = !product.IsDeleted;

            // Recover one of the categories if all of the product's categories are deleted
            if (product.ProductCategories.All(pc => pc.Category.IsDeleted))
            {
                var productCategory = product.ProductCategories.FirstOrDefault();
                if (productCategory != null)
                {
                    await this.categoriesService.RecoverSimpleAsync(productCategory.CategoryId);
                }
            }

            await this.productsRepository.SaveChangesAsync();

            return product.Title;
        }

        // [Hard Delete] Delete a product by Id
        public async Task<string> HardDeleteAsync(int id)
        {
            var product = await this.productsRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return null;
            }

            this.productsRepository.HardDelete(product);
            await this.productsRepository.SaveChangesAsync();

            return product.Title;
        }

        // Edit a product
        public async Task EditAsync(AdminProductEditViewModel viewModel)
        {
            var product = await this.productsRepository
                .AllWithDeleted()
                .Include(p => p.Images)
                .FirstOrDefaultAsync(c => c.Id == viewModel.Id);

            var editedImages = new List<Image>();
            await this.EditImagesAsync(viewModel, editedImages, product.Id);

            var editedProductCategory = new ProductCategory()
            {
                CategoryId = int.Parse(viewModel.Categories[0].Name),
                ProductId = product.Id,
            };

            product.Name = viewModel.Name;
            product.Title = viewModel.Title;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;
            product.DiscountPrice = viewModel.DiscountPrice;
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
                throw new Exception(
                    string.Format(GlobalConstants.Product.OnCreateProductNotUniqueError, inputModel.Name));
            }

            var product = new Product()
            {
                CreatedOn = DateTime.UtcNow,
                Name = inputModel.Name,
                Title = inputModel.Title,
                Description = inputModel.Description,
                Price = inputModel.Price,
                DiscountPrice = inputModel.DiscountPrice,
                Quantity = inputModel.Quantity,
                Height = inputModel.Height,
                Width = inputModel.Width,
            };

            // Include product images
            product.Images = await this.FillImagesAsync(inputModel, product.Id);

            // Include productCategories
            product.ProductCategories = this.FillProductCategories(inputModel, product.Id);

            await this.productsRepository.AddAsync(product);
            await this.productsRepository.SaveChangesAsync();
        }

        // Get the price of the most expensive product
        public async Task<decimal> MostExpensiveProductPrice()
        {
            decimal price = await this.productsRepository.All().AnyAsync()
                ? await this.productsRepository.All().MaxAsync(p => p.Price)
                : 0;

            return price;
        }

        // Get the price of the cheapest product
        public async Task<decimal> LeastExpensiveProductPrice()
        {
            decimal price = await this.productsRepository.All().AnyAsync()
                ? await this.productsRepository.All().MinAsync(p => p.DiscountPrice ?? p.Price)
                : 0;

            return price;
        }

        private static IQueryable<Product> FilterByPrice(int? minPrice, int? maxPrice, IQueryable<Product> products)
        {
            if (minPrice != null && maxPrice != null)
            {
                products = products
                    .Where(p => (p.DiscountPrice ?? p.Price) >= minPrice.Value
                                && (p.DiscountPrice ?? p.Price) <= maxPrice.Value);
            }

            return products;
        }

        private static IQueryable<Product> SortProducts(SortBy sortBy, IQueryable<Product> products)
        {
            products = sortBy switch
            {
                SortBy.Newest => products.OrderByDescending(p => p.CreatedOn),
                SortBy.PriceDescending => products.OrderByDescending(p => p.DiscountPrice ?? p.Price),
                SortBy.PriceAscending => products.OrderBy(p => p.DiscountPrice ?? p.Price),
                _ => products.OrderByDescending(p => p.OrderProducts.Count),
            };
            return products;
        }

        private async Task EditImagesAsync(AdminProductEditViewModel viewModel, List<Image> editedImages, int productId)
        {
            for (var i = 0; i < viewModel.Images.Count; i++)
            {
                var name = $"{viewModel.Name}{i}";

                // Remove image from Cloudinary if it's marked for deletion
                if (viewModel.Images[i].ToDelete && viewModel.Images[i].ImgUrl != null)
                {
                    AppCloudinary.DeleteImage(this.cloudinary, name);
                }
                else
                {
                    string imageUrl;
                    if (viewModel.Images[i].Image != null)
                    {
                        var image = viewModel.Images[i];
                        imageUrl =
                            await AppCloudinary.UploadImage(this.cloudinary, image.Image, name);
                    }
                    else
                    {
                        imageUrl = viewModel.Images[i].ImgUrl;
                    }

                    if (imageUrl != null)
                    {
                        editedImages.Add(new Image()
                        {
                            CreatedOn = DateTime.UtcNow,
                            ImgUrl = imageUrl,
                            ProductId = productId,
                        });
                    }
                }
            }
        }

        private ICollection<ProductCategory> FillProductCategories(AdminProductInputModel inputModel, int productId)
        {
            return inputModel
                .Categories
                .Select(pc => new ProductCategory()
                {
                    ProductId = productId,
                    CategoryId = int.Parse(inputModel.Categories[0].Name),
                })
                .ToList();
        }

        private async Task<ICollection<Image>> FillImagesAsync(AdminProductInputModel inputModel, int productId)
        {
            var images = new List<Image>();
            for (var i = 0; i < inputModel.Images.Count; i++)
            {
                var image = inputModel.Images[i];
                var name = $"{inputModel.Name}{i}";
                var imageUrl =
                    await AppCloudinary.UploadImage(this.cloudinary, image.Image, name);
                images.Add(new Image()
                {
                    CreatedOn = DateTime.UtcNow,
                    ImgUrl = imageUrl,
                    ProductId = productId,
                });
            }

            return images;
        }
    }
}
