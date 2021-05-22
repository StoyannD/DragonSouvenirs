namespace DragonSouvenirs.Web.ViewModels.Components.CartProductsSimpleComponent
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CartProductsSimpleViewModel : IMapFrom<CartProduct>
    {
        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice => this.Quantity * this.ProductPrice;
    }
}
