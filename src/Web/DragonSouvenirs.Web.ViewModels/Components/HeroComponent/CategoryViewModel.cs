namespace DragonSouvenirs.Web.ViewModels.Components.HeroComponent
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public string Title { get; set; }

        public string Name { get; set; }
    }
}
