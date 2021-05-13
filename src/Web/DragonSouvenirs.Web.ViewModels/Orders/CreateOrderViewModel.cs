﻿namespace DragonSouvenirs.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CreateOrderViewModel : IMapFrom<Order>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.Order.UserFullNameMaxLength,
            MinimumLength = GlobalConstants.Order.UserFullNameMinLength,
            ErrorMessage = GlobalConstants.Order.UserFullNameError)]
        public string UserFullName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = GlobalConstants.Order.UserEmailError)]
        public string UserEmail { get; set; }

        [Required]
        [Phone(ErrorMessage = GlobalConstants.Order.InvoiceNumberError)]
        public string InvoiceNumber { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.Order.ShippingAddressMaxLength,
            MinimumLength = GlobalConstants.Order.ShippingAddressMinLength,
            ErrorMessage = GlobalConstants.Order.ShippingAddressError)]
        public string ShippingAddress { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string UserId { get; set; }
    }
}