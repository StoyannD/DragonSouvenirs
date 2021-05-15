namespace DragonSouvenirs.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DragonSouvenirs.Data.Common.Models;
    using DragonSouvenirs.Data.Models.Enums;

    public class Order : BaseDeletableModel<int>
    {
        public Order()
        {
            this.OrderProducts = new HashSet<OrderProduct>();
        }

        public string ShippingAddress { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        public DateTime? DateOfDelivery { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string UserEmail { get; set; }

        public string UserFullName { get; set; }

        public string InvoiceNumber { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
