namespace DragonSouvenirs.Web.ViewModels.Components
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class ProductCategoryViewModel : IMapFrom<ProductCategory>
    {
        public string CategoryName { get; set; }
    }
}
