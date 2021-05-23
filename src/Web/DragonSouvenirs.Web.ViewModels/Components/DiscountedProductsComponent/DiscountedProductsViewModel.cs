namespace DragonSouvenirs.Web.ViewModels.Components.DiscountedProductsComponent
{
    using System.Linq;

    using AutoMapper;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class DiscountedProductsViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string TitleLink => this.Title.Replace(' ', '-');

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public string CategoryName { get; set; }

        public string CategoryTitle { get; set; }

        public string PrimaryImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, DiscountedProductsViewModel>()
                .ForMember(m => m.CategoryName, options =>
                {
                    options.MapFrom(
                        p => string.Join(" ", p.ProductCategories
                            .Select(pc => pc.Category.Name ?? string.Empty)
                            .ToList()));
                })
                .ForMember(m => m.CategoryTitle, options =>
                {
                    options.MapFrom(
                        p => string.Join(" ", p.ProductCategories
                            .Select(pc => pc.Category.Title ?? string.Empty)
                            .ToList()));
                })
                .ForMember(m => m.PrimaryImageUrl, options =>
                {
                    options.MapFrom(
                        p => p.Images.FirstOrDefault().ImgUrl);
                });
        }
    }
}
