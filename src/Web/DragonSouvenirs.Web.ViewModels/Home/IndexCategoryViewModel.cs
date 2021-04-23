namespace DragonSouvenirs.Web.ViewModels.Categories
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class IndexCategoryViewModel : IMapFrom<Category>
    {
        public string Title { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public int ProductCategoriesCount { get; set; }

        public string Url => $"{this.Name.Replace(' ', '-')}";
    }
}
