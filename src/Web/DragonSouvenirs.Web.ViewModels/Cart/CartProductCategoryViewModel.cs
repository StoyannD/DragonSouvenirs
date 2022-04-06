namespace DragonSouvenirs.Web.ViewModels.Cart
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CartProductCategoryViewModel : IMapFrom<ProductCategory>
    {
        public string CategoryTitle { get; set; }
    }
}
