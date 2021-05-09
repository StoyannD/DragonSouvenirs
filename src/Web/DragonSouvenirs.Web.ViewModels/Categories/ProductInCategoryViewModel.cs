namespace DragonSouvenirs.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class ProductInCategoryViewModel : IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public IEnumerable<ImageViewModel> Images { get; set; }
    }
}
