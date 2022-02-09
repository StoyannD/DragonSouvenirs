namespace DragonSouvenirs.Web.ViewModels.Components.HeroComponent
{
    using System.Collections.Generic;

    public class CategoriesViewModel
    {
        public bool IsIndex { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
