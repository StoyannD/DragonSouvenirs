namespace DragonSouvenirs.Web.ViewModels.Components.HeroComponent
{
    using System.Collections.Generic;

    public class CategoriesViewModel
    {
        public bool IsIndex { get; set; }

        public string CategoryName { get; set; }

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
