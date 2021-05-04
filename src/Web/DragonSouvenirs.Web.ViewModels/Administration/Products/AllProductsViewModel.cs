namespace DragonSouvenirs.Web.ViewModels.Administration.Products
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AllProductsViewModel
    {
        public IEnumerable<AdminProductViewModel> Products { get; set; }
    }
}
