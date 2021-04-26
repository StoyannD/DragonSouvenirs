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

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public ProductsService(IDeletableEntityRepository<Product> productsRepository)
        {
            this.productsRepository = productsRepository;
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

        public async Task<IEnumerable<T>> GetAllByCategoryNameAsync<T>(string name)
        {
            var products = await this.productsRepository
                .All()
                .OrderBy(p => p.CreatedOn)
                .Where(p => p.ProductCategories.Any(pc => pc.Category.Name == name && pc.ProductId == p.Id))
                .To<T>()
                .ToListAsync();

            return products;
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var product = await this.productsRepository
                .All()
                .Where(p => p.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return product;
        }
    }
}
