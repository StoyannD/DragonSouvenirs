namespace DragonSouvenirs.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Models;

    public class ProductsCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ProductCategories.Any())
            {
                return;
            }

            var productsCategories = new List<ProductCategory>()
            {
                new ProductCategory() { CategoryId = 1, ProductId = 1 },
                new ProductCategory() { CategoryId = 1, ProductId = 2 },
                new ProductCategory() { CategoryId = 2, ProductId = 3 },
                new ProductCategory() { CategoryId = 2, ProductId = 4 },
                new ProductCategory() { CategoryId = 3, ProductId = 5 },
                new ProductCategory() { CategoryId = 3, ProductId = 6 },
            };

            await dbContext.ProductCategories.AddRangeAsync(productsCategories);
        }
    }
}
