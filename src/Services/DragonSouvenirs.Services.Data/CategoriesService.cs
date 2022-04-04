namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        // Get all categories, ordered by Title
        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var categories = await this.categoriesRepository
                .All()
                .OrderBy(c => c.Title)
                .To<T>()
                .ToListAsync();

            return categories;
        }

        // Get DELETED categories, ordered by Title
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

        // Get all WITH DELETED categories, ordered by Title
        public async Task<IEnumerable<T>> GetAllAdminAsync<T>()
        {
            var categories = await this.categoriesRepository
                .AllWithDeleted()
                .OrderBy(c => c.Title)
                .To<T>()
                .ToListAsync();

            return categories;
        }

        // Get all categories of a product BY ProductId
        public async Task<IEnumerable<T>> GetAllByProductIdAsync<T>(int? id)
        {
            var categories = await this.categoriesRepository
                .AllWithDeleted()
                .Where(c => c.ProductCategories.Any(pc => pc.ProductId == id.Value))
                .To<T>()
                .ToListAsync();

            return categories;
        }

        // Get category by Name
        public async Task<T> GetByNameAsync<T>(string name)
        {
            var category = await this.categoriesRepository
                .All()
                .Where(c => c.Name == name)
                .To<T>()
                .FirstOrDefaultAsync();

            return category;
        }

        // Get category by Id
        public async Task<T> GetByIdAsync<T>(int? id)
        {
            var category = await this.categoriesRepository
                .AllWithDeleted()
                .Where(c => c.Id == id.Value)
                .To<T>()
                .FirstOrDefaultAsync();

            return category;
        }

        // Delete or Recover a category by Id
        public async Task<string> DeleteRecoverAsync(int id)
        {
            var category = await this.GetByIdWithDeletedAsync(id);

            if (category == null)
            {
                return null;
            }

            category.IsDeleted = !category.IsDeleted;
            category.DeletedOn = category.IsDeleted
                ? DateTime.UtcNow
                : category.DeletedOn;

            var products = this.productsRepository
                    .AllWithDeleted()
                    .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == id));

            foreach (var product in products)
            {
                product.IsDeleted = !product.IsDeleted;
                product.DeletedOn = product.IsDeleted
                    ? DateTime.UtcNow
                    : product.DeletedOn;
            }

            await this.productsRepository.SaveChangesAsync();
            await this.categoriesRepository.SaveChangesAsync();

            return category.Title;
        }

        // Edit category
        public async Task EditAsync(AdminCategoryEditViewModel viewModel)
        {
            var category = await this.GetByIdWithDeletedAsync(viewModel.Id);

            if (category == null)
            {
                return;
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

        // Create category
        public async Task CreateAsync(AdminCategoryInputModel inputModel)
        {
            if (await this.categoriesRepository
                .AllWithDeleted().AnyAsync(c => c.Name == inputModel.Name))
            {
                throw new Exception(
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

        // Get category by Id (Including Deleted)
        private async Task<Category> GetByIdWithDeletedAsync(int id)
        {
            return await this.categoriesRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
