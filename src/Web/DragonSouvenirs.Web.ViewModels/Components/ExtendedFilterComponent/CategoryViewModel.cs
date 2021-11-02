namespace DragonSouvenirs.Web.ViewModels.Components.ExtendedFilterComponent
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public bool IsSelected { get; set; }
    }
}
