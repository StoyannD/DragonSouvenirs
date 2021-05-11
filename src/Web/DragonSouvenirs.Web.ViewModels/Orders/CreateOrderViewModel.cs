namespace DragonSouvenirs.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CreateOrderViewModel : IMapFrom<Order>
    {
        public int Id { get; set; }

        [Required]
        public string UserUserName { get; set; }

        public string UserDefaultShippingAddress { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string UserId { get; set; }
    }
}
