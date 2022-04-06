namespace DragonSouvenirs.Web.ViewModels.Components.ExtendedFilterComponent
{
    using System.Collections.Generic;

    public class ExtendedFilterViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public string CurrentCategory { get; set; }

        public string SearchString { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }
    }
}
