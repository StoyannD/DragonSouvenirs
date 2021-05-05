namespace DragonSouvenirs.Data.Models
{
    using System;

    using DragonSouvenirs.Data.Common.Models;

    public class CartProduct : IDeletableEntity
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public int CartId { get; set; }

        public Cart Cart { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
