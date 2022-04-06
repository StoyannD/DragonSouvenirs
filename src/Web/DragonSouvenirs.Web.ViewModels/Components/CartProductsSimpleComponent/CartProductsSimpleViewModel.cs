namespace DragonSouvenirs.Web.ViewModels.Components.CartProductsSimpleComponent
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CartProductsSimpleViewModel : IMapFrom<CartProduct>
    {
        public string ProductTitle { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal? ProductDiscountPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice =>
            this.Quantity * (this.ProductDiscountPrice ?? this.ProductPrice);

        public decimal TotalPriceBeforeDiscounts =>
            this.Quantity * this.ProductPrice;
    }
}
