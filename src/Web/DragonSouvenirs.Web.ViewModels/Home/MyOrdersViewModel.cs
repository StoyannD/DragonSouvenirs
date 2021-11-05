namespace DragonSouvenirs.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class MyOrdersViewModel
    {
        public IEnumerable<MyOrderViewModel> Orders { get; set; }
    }
}
