namespace DragonSouvenirs.Web.ViewModels.Products
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class FavouriteProductViewModel : IMapFrom<FavouriteProduct>
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductTitle { get; set; }

        public string ProductNameLink => this.ProductName.Replace(' ', '-');

        public decimal ProductPrice { get; set; }

        public decimal? ProductDiscountPrice { get; set; }

        public IEnumerable<ImageViewModel> ProductImages { get; set; }

        public string ProductCategoryName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ProductCategory, FavouriteProductViewModel>()
                .ForMember(m => m.ProductCategoryName, options =>
                {
                    options.MapFrom(
                        p => string.Join(" ", p.Product.ProductCategories
                            .Select(pc => pc.Category.Name ?? string.Empty)
                            .ToList()));
                });
        }
    }
}
