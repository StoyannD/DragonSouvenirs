namespace DragonSouvenirs.Web.ViewModels.Administration.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class AdminProductViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int OrderProductsCount { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IEnumerable<ProductCategoriesViewModel> ProductCategories { get; set; }

        public string ProductCategoriesNames
        {
            get
            {
                if (this.ProductCategories == null)
                {
                    return string.Empty;
                }

                var categoriesNames = string.Join(" ", this.ProductCategories
                    .Select(c => c.Category.Title ?? string.Empty)
                    .ToList());

                return categoriesNames;
            }
        }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, AdminProductViewModel>()
                .ForMember(m => m.ImageUrl, options =>
                {
                    options.MapFrom(
                        p => p.Images
                            .First().ImgUrl);
                });
        }
    }
}
