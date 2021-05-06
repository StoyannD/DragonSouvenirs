namespace DragonSouvenirs.Web.ViewModels.Cart
{
    using System.Collections.Generic;
    using System.Linq;

    public class CartAllProductsViewModel
    {
        public IEnumerable<CartProductViewModel> CartProducts { get; set; }

        public decimal? TotalPrice =>
            this.CartProducts
                .Sum(cp => cp.TotalPrice);
    }
}
