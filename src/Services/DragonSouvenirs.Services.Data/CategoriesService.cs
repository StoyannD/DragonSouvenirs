namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Data.Common;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels.Administration.Categories;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Product> productsRepository;
        private readonly Cloudinary cloudinary;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Product> productsRepository,
            Cloudinary cloudinary)
        {
            this.categoriesRepository = categoriesRepository;
            this.productsRepository = productsRepository;
            this.cloudinary = cloudinary;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var categories = await this.categoriesRepository
                .All()
                .OrderBy(c => c.Title)
                .To<T>()
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<T>> GetDeletedAsync<T>()
        {
            var categories = await this.categoriesRepository
                .AllWithDeleted()
                .Where(c => c.IsDeleted)
                .OrderBy(c => c.Title)
                .To<T>()
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<T>> GetAllAdminAsync<T>()
        {
            var categories = await this.categoriesRepository
                .AllWithDeleted()
                .OrderBy(c => c.Title)
                .To<T>()
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<T>> GetAllByProductIdAsync<T>(int? id)
        {
            var categories = await this.categoriesRepository
                .AllWithDeleted()
                .Where(c => c.ProductCategories.Any(pc => pc.ProductId == id.Value))
                .To<T>()
                .ToListAsync();

            return categories;
        }

        public async Task<T> GetByNameAsync<T>(string name)
        {
            var category = await this.categoriesRepository
                .All()
                .Where(c => c.Name == name)
                .To<T>()
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<T> GetByIdAsync<T>(int? id)
        {
            var category = await this.categoriesRepository
                .AllWithDeleted()
                .Where(c => c.Id == id.Value)
                .To<T>()
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<string> DeleteRecoverAsync(int id)
        {
            var category = await this.categoriesRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(c => c.Id == id);

            var products = this.productsRepository
                    .AllWithDeleted()
                    .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == id));

            if (category == null)
            {
                throw new NullReferenceException();
            }

            foreach (var product in products)
            {
                product.IsDeleted = !product.IsDeleted;
            }

            category.IsDeleted = !category.IsDeleted;

            await this.productsRepository.SaveChangesAsync();
            await this.categoriesRepository.SaveChangesAsync();

            return category.Title;
        }

        public async Task EditAsync(AdminCategoryEditViewModel viewModel)
        {
            var category = await this.categoriesRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(c => c.Id == viewModel.Id);

            if (category == null)
            {
                throw new NullReferenceException();
            }

            if (viewModel.Image != null)
            {
                var imageUrl =
                    await AppCloudinary.UploadImage(this.cloudinary, viewModel.Image, viewModel.Name);
                category.ImageUrl = imageUrl;
            }

            category.Name = viewModel.Name;
            category.Title = viewModel.Title;
            category.Content = viewModel.Content;

            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task CreateAsync(AdminCategoryInputModel inputModel)
        {
            if (await this.categoriesRepository
                .AllWithDeleted().AnyAsync(c => c.Name == inputModel.Name))
            {
                throw new AmbiguousImplementationException(
                    string.Format(GlobalConstants.Category.OnCreateCategoryNotUniqueError, inputModel.Name));
            }

            var imageUrl =
                await AppCloudinary.UploadImage(this.cloudinary, inputModel.Image, inputModel.Name);

            var category = new Category
            {
                Name = inputModel.Name,
                Title = inputModel.Title,
                Content = inputModel.Content,
                ImageUrl = imageUrl,
                CreatedOn = DateTime.UtcNow,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();
        }
    }
}
