namespace DragonSouvenirs.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DragonSouvenirs.Web.ViewModels.Categories;

    public class IndexViewModel
    {
        public IEnumerable<IndexCategoryViewModel> Categories { get; set; }
    }
}
