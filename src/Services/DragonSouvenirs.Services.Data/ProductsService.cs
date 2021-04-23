namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<Product> productsRepository;

        public ProductsService(IDeletableEntityRepository<Product> productsRepository)
        {
            this.productsRepository = productsRepository;
        }

        public IEnumerable<T> GetByCategoryName<T>(string name)
        {
            var products = this.productsRepository
                .All()
                .OrderBy(p => p.CreatedOn)
                .Where(p => p.ProductCategories.Any(pc => pc.Category.Name == name && pc.ProductId == p.Id))
                .To<T>()
                .ToList();

            return products;
        }

        public T GetById<T>(int id)
        {
            var product = this.productsRepository
                .All()
                .Where(p => p.Id == id)
                .To<T>()
                .FirstOrDefault();

            return product;
        }
    }
}
