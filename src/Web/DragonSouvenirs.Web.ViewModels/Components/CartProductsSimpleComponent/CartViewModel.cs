using System.Linq;

namespace DragonSouvenirs.Web.ViewModels.Components.CartProductsSimpleComponent
{
    using System.Collections.Generic;

    public class CartViewModel
    {
        public IEnumerable<CartProductsSimpleViewModel> Products { get; set; }

        public decimal TotalPrice =>
            Products.Sum(p => p.TotalPrice);

        public decimal TotalPriceBeforeDiscounts =>
            Products.Sum(p => p.TotalPriceBeforeDiscounts);

    }
}
