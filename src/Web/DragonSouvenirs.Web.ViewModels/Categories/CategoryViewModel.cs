namespace DragonSouvenirs.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public CategoryPaginationInfo CategoryPaginationInfo { get; set; }

        public IEnumerable<ProductInCategoryViewModel> Products { get; set; }
    }
}
