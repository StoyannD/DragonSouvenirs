namespace DragonSouvenirs.Data.Models
{
    using DragonSouvenirs.Data.Common.Models;

    public class CartProduct : BaseDeletableModel<int>
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public int CartId { get; set; }

        public Cart Cart { get; set; }
    }
}
