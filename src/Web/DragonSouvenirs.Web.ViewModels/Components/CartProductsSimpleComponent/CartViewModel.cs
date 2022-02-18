namespace DragonSouvenirs.Web.ViewModels.Components.CartProductsSimpleComponent
{
    using System.Collections.Generic;
    using System.Linq;

    public class CartViewModel
    {
        public decimal PersonalDiscountPercentage { get; set; }

        public IEnumerable<CartProductsSimpleViewModel> Products { get; set; }

        public decimal TotalPrice =>
            Products.Sum(p => p.TotalPrice)
            * (this.PersonalDiscountPercentage == 0
                ? 1
                : 1 - (this.PersonalDiscountPercentage / 100));

        public decimal TotalPriceBeforeDiscounts =>
            Products.Sum(p => p.TotalPriceBeforeDiscounts);
    }
}
