namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime;
    using System.Text;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels.Administration.Categories;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
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

            if (category == null)
            {
                return null;
            }

            category.IsDeleted = !category.IsDeleted;
            await this.categoriesRepository.SaveChangesAsync();

            return category.Title;
        }

        public async Task EditAsync(AdminCategoryEditViewModel viewModel)
        {
            var category = await this.categoriesRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(c => c.Id == viewModel.Id);

            category.Name = viewModel.Name;
            category.Title = viewModel.Title;
            category.Content = viewModel.Content;
            category.ImageUrl = viewModel.ImageUrl;

            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task CreateAsync(AdminCategoryInputModel inputModel)
        {
            if (this.categoriesRepository.AllWithDeleted().Any(c => c.Name == inputModel.Name))
            {
                throw new AmbiguousImplementationException(
                    string.Format(GlobalConstants.Category.OnCreateCategoryNotUniqueError, inputModel.Name));
            }

            var category = new Category
            {
                Name = inputModel.Name,
                Title = inputModel.Title,
                Content = inputModel.Content,
                ImageUrl = inputModel.ImageUrl,
                CreatedOn = DateTime.UtcNow,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();
        }
    }
}
