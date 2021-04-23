namespace DragonSouvenirs.Data.Models
{
    using System.Collections.Generic;

    using DragonSouvenirs.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.ProductCategories = new HashSet<ProductCategory>();
        }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
