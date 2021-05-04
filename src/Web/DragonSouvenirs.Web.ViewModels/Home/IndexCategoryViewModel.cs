namespace DragonSouvenirs.Web.ViewModels.Home
{
    using System.Linq;

    using AutoMapper;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class IndexCategoryViewModel : IMapFrom<Category>, IHaveCustomMappings
    {
    public string Title { get; set; }

    public string Name { get; set; }

    public string Content { get; set; }

    public string ImageUrl { get; set; }

    public int ProductCategoriesCount { get; set; }

    public string Url => $"{this.Name.Replace(' ', '-')}";

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Category, IndexCategoryViewModel>()
            .ForMember(m => m.ProductCategoriesCount, options =>
            {
                options.MapFrom(c => c.ProductCategories
                    .Count(p => !p.Product.IsDeleted));
            });
    }
    }
}
