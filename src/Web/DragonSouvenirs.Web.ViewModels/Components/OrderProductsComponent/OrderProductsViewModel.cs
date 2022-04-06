namespace DragonSouvenirs.Web.ViewModels.Components.OrderProductsComponent
{
    using System.Collections.Generic;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class OrderProductsViewModel : IMapFrom<OrderProduct>
    {
        public string ProductTitle { get; set; }

        public string ProductDescription { get; set; }

        public string ProductDescriptionShort
        {
            get
            {
                if (this.ProductDescription == null)
                {
                    return string.Empty;
                }

                var content = this.ProductDescription.Length > 50
                    ? this.ProductDescription.Substring(0, 50) + "..."
                    : this.ProductDescription;
                return content;
            }
        }

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        public int ProductHeight { get; set; }

        public int ProductWidth { get; set; }

        public IEnumerable<ProductImageViewModel> ProductImages { get; set; }

        public IEnumerable<ProductCategoryViewModel> ProductProductCategories { get; set; }

        public decimal TotalPrice => this.Quantity * this.ProductPrice;
    }
}
