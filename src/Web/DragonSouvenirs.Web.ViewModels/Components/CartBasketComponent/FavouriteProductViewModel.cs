namespace DragonSouvenirs.Web.ViewModels.Components.CartBasketComponent
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class FavouriteProductViewModel : IMapFrom<FavouriteProduct>
    {
        public string UserId { get; set; }

        public int ProductId { get; set; }
    }
}
