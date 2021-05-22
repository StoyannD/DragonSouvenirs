﻿using AutoMapper;

namespace DragonSouvenirs.Web.ViewModels.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class ProductInCategoryViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string TitleLink => this.Title.Replace(' ', '-');

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public IEnumerable<ImageViewModel> Images { get; set; }

        public string CategoryName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductInCategoryViewModel>()
                .ForMember(m => m.CategoryName, options =>
                {
                    options.MapFrom(
                        p => string.Join(" ", p.ProductCategories
                            .Select(pc => pc.Category.Name ?? string.Empty)
                            .ToList()));
                });
        }
    }
}
