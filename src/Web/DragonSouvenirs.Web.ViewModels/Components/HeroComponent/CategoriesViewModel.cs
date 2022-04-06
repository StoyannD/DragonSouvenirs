namespace DragonSouvenirs.Web.ViewModels.Components.HeroComponent
{
    using System.Collections.Generic;

    using DragonSouvenirs.Common.Enums;

    public class CategoriesViewModel
    {
        public string CategoryName { get; set; }

        public SortBy SortBy { get; set; }

        public int? CurrentPage { get; set; }

        public int? ProductsPerPage { get; set; }

        public bool IsIndex { get; set; }

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
