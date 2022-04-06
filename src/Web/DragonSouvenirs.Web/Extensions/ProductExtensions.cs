namespace DragonSouvenirs.Web.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DragonSouvenirs.Web.ViewModels.Categories;

    public static class ProductExtensions
    {
        public static IEnumerable<ProductInCategoryViewModel> FilterBySearchString(
            this IEnumerable<ProductInCategoryViewModel> products, string searchString)
        {
            if (searchString != null)
            {
                var searchStringArr = searchString
                    .Split(new[] { ",", ".", " ", "\\", "/", "|", "!", "?" }, StringSplitOptions.RemoveEmptyEntries);

                return products.Where(p =>
                    searchStringArr.All(ss => p.Title.ToLower().Contains(ss.ToLower())));
            }

            return products;
        }
    }
}
