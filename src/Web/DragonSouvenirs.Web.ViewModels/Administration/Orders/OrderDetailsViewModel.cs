namespace DragonSouvenirs.Web.ViewModels.Administration.Orders
{
    using System;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Models.Enums;
    using DragonSouvenirs.Services.Mapping;

    public class OrderDetailsViewModel : IMapFrom<Order>
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string ClientFullName { get; set; }

        public string UserEmail { get; set; }

        public string InvoiceNumber { get; set; }

        public string ShippingAddress { get; set; }

        public string Notes { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        public DateTime? DateOfDelivery { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public OfficeBrands OfficeBrand { get; set; }
    }
}
