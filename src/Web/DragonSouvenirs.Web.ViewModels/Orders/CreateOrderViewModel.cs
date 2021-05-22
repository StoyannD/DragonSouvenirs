namespace DragonSouvenirs.Web.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CreateOrderViewModel : IMapFrom<Order>
    {
        public int Id { get; set; }

        public string UserFullName { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.Order.UserFullNameMaxLength,
            MinimumLength = GlobalConstants.Order.UserFullNameMinLength,
            ErrorMessage = GlobalConstants.Order.UserFullNameError)]
        public string FirstName
            => this.UserFullName.Split(' ').First();

        [Required]
        [StringLength(
            GlobalConstants.Order.UserFullNameMaxLength,
            MinimumLength = GlobalConstants.Order.UserFullNameMinLength,
            ErrorMessage = GlobalConstants.Order.UserFullNameError)]
        public string LastName
            => this.UserFullName[this.UserFullName.IndexOf(" ", StringComparison.Ordinal)..];

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

        [StringLength(
            GlobalConstants.Order.NotesMaxLength,
            MinimumLength = GlobalConstants.Order.NotesMinLength,
            ErrorMessage = GlobalConstants.Order.NotesError)]
        public string Notes { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string UserId { get; set; }
    }
}
