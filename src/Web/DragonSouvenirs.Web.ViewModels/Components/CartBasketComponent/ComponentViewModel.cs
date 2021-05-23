namespace DragonSouvenirs.Web.ViewModels.Components.CartBasketComponent
{
    using System.Collections.Generic;
    using System.Linq;

    public class ComponentViewModel
    {
        public CartBasketViewModel Cart { get; set; }

        public IEnumerable<FavouriteProductViewModel> FavouriteProducts { get; set; }

        public int FavouriteProductsCount => this.FavouriteProducts.Count();
    }
}
