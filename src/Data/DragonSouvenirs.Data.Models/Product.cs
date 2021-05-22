namespace DragonSouvenirs.Data.Models
{
    using System.Collections.Generic;

    using DragonSouvenirs.Data.Common.Models;

    public class Product : BaseDeletableModel<int>
    {
        public Product()
        {
            this.OrderProducts = new HashSet<OrderProduct>();
            this.ProductCategories = new HashSet<ProductCategory>();
            this.Images = new HashSet<Image>();
            this.CartProducts = new HashSet<CartProduct>();
        }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public int Quantity { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<CartProduct> CartProducts { get; set; }
    }
}
