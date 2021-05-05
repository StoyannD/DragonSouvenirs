namespace DragonSouvenirs.Web.ViewModels.Cart
{
    using System.Collections.Generic;

    public class CartAllProductsViewModel
    {
        public IEnumerable<CartProductViewModel> CartProducts { get; set; }
    }
}
