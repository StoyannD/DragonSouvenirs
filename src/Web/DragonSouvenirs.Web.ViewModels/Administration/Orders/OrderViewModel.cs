﻿namespace DragonSouvenirs.Web.ViewModels.Administration.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Models.Enums;
    using DragonSouvenirs.Services.Mapping;

    public class OrderViewModel : IMapFrom<Order>
    {
        public int Id { get; set; }

        public string UserEmail { get; set; }

        public string UserFullName { get; set; }

        public string UserUserName { get; set; }

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

        public decimal DeliveryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
