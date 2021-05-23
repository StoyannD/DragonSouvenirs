namespace DragonSouvenirs.Web.ViewModels.Products
{
    using System.Collections.Generic;

    using DragonSouvenirs.Web.ViewModels.Categories;

    public class FavouriteProductsViewModel
    {
        public IEnumerable<FavouriteProductViewModel> Products { get; set; }
    }
}
