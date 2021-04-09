namespace DragonSouvenirs.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Models;

    public class ProductsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Products.Any())
            {
                return;
            }

            var products = new List<Product>()
            {
                new Product() { Name = "Часовник \"Кон\"", Description = "Ръчно изработен часовник от дърво във формата на кон", Price = 25.5M, Quantity = 10, Height = 20, Width = 20 },
                new Product() { Name = "Часовник \"Кораб\"", Description = "Ръчно изработен часовник от дърво във формата на кораб", Price = 35.5M, Quantity = 12, Height = 30, Width = 20 },
                new Product() { Name = "Албум Голям", Description = "Ръчно изработен Албум от дърво", Price = 15M, Quantity = 8, Height = 40, Width = 20 },
                new Product() { Name = "Албум Малък", Description = "Ръчно изработен Албум от дърво", Price = 10M, Quantity = 14, Height = 20, Width = 20 },
                new Product() { Name = "Кутия за вино с капитан", Description = "Ръчно изработена кутия за вино с фигурка на капитан", Price = 40M, Quantity = 9, Height = 45, Width = 15 },
                new Product() { Name = "Кутия за вино с кораб", Description = "Ръчно изработена кутия за вино с фигурка на кораб", Price = 40M, Quantity = 11, Height = 45, Width = 15 },
            };

            await dbContext.Products.AddRangeAsync(products);
        }
    }
}
