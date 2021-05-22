namespace DragonSouvenirs.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class ShopViewModel
    {
        public CategoryPaginationInfo CategoryPaginationInfo { get; set; }

        public IEnumerable<ProductInCategoryViewModel> Products { get; set; }
    }
}
