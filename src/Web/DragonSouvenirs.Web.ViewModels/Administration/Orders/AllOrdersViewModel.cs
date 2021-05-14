namespace DragonSouvenirs.Web.ViewModels.Administration.Orders
{
    using System.Collections.Generic;

    public class AllOrdersViewModel
    {
        public IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
