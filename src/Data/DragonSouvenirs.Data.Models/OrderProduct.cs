namespace DragonSouvenirs.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OrderProduct
    {
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
