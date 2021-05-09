namespace DragonSouvenirs.Web.ViewModels.Administration.Products
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CategoriesDropdownViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
