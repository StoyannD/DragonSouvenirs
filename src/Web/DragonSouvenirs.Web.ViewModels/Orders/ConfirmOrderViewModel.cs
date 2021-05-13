﻿namespace DragonSouvenirs.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class ConfirmOrderViewModel : IMapFrom<Order>
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string UserFullName { get; set; }

        public string UserEmail { get; set; }

        public string InvoiceNumber { get; set; }

        public string ShippingAddress { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal CartTotalPrice { get; set; }
    }
}