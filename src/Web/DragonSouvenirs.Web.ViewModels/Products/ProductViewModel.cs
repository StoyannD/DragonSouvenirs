namespace DragonSouvenirs.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class ProductViewModel : IMapFrom<Product>, IHaveCustomMappings
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

        public string CategoryName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductViewModel>()
                .ForMember(m => m.CategoryName, options =>
                {
                    options.MapFrom(
                        p => p.ProductCategories
                            .Select(pc => pc.Category.Title)
                            .FirstOrDefault());
                });
        }
    }
}
