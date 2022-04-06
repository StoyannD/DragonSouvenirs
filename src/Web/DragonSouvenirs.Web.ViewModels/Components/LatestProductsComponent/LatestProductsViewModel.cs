namespace DragonSouvenirs.Web.ViewModels.Components.LatestProductsComponent
{
    using System.Linq;

    using AutoMapper;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class LatestProductsViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string NameLink => this.Name.Replace(' ', '-');

        public decimal Price { get; set; }

        public string ImgUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, LatestProductsViewModel>()
                .ForMember(m => m.ImgUrl, options =>
                {
                    options.MapFrom(
                        p => p.Images.FirstOrDefault().ImgUrl);
                });
        }
    }
}
