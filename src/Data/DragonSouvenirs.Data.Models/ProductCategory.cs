namespace DragonSouvenirs.Data.Models
{
    using System;

    using DragonSouvenirs.Data.Common.Models;

    public class ProductCategory
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
