namespace DragonSouvenirs.Web.ViewModels.Cart
{
    using System.Collections.Generic;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CartProductViewModel : IMapFrom<CartProduct>
    {
        public int CartId { get; set; }

        public string CartUserId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductQuantity { get; set; }

        public int ProductHeight { get; set; }

        public int ProductWidth { get; set; }

        public IEnumerable<CartProductImageViewModel> ProductImages { get; set; }

        public IEnumerable<CartProductCategoryViewModel> ProductProductCategories { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice => this.Quantity * this.ProductPrice;
    }
}
