namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;
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

        public async Task<T> GetByNameAsync<T>(string name)
        {
            var category = await this.categoriesRepository
                .All()
                .Where(c => c.Name == name)
                .To<T>()
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var category = await this.categoriesRepository
                .AllWithDeleted()
                .Where(c => c.Id == id)
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
    }
}
