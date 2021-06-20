namespace DragonSouvenirs.Web.ViewModels.Administration.Orders
{
    using System;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Models.Enums;
    using DragonSouvenirs.Services.Mapping;

    public class OrderViewModel : IMapFrom<Order>
    {
        public int Id { get; set; }

        public string UserEmail { get; set; }

        public string ClientFullName { get; set; }

        public string ShippingAddress { get; set; }

        public string ShippingAddressShort
        {
            get
            {
                if (this.ShippingAddress == null)
                {
                    return string.Empty;
                }

                var content = this.ShippingAddress.Length > 30
                    ? this.ShippingAddress.Substring(0, 30) + "..."
                    : this.ShippingAddress;
                return content;
            }
        }

        public OrderStatus OrderStatus { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DateOfDelivery { get; set; }

        public OfficeBrands OfficeBrand { get; set; }
    }
}
