namespace DragonSouvenirs.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var categories = new List<Category>()
            {
                new Category() { Title = "Часовници", Content = "Ръчно изработени часовници от дърво." },
                new Category() { Title = "Албуми", Content = "Ръчно изработени Албуми от дърво." },
                new Category() { Title = "Вина", Content = "Ръчно изработени кутии за вино от дърво." },
            };

            await dbContext.Categories.AddRangeAsync(categories);
        }
    }
}
