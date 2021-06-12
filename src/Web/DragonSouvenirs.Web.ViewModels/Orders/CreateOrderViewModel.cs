namespace DragonSouvenirs.Web.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Models.Enums;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels.Offices;

    public class CreateOrderViewModel : IMapFrom<Order>
    {
        public int Id { get; set; }

        public string UserFullName { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.Order.UserFullNameMaxLength,
            MinimumLength = GlobalConstants.Order.UserFullNameMinLength,
            ErrorMessage = GlobalConstants.Order.UserFullNameError)]

        public string FirstName { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.Order.UserFullNameMaxLength,
            MinimumLength = GlobalConstants.Order.UserFullNameMinLength,
            ErrorMessage = GlobalConstants.Order.UserFullNameError)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = GlobalConstants.Order.UserEmailError)]
        public string UserEmail { get; set; }

        [Required]
        [Phone(ErrorMessage = GlobalConstants.Order.InvoiceNumberError)]
        public string InvoiceNumber { get; set; }

        [Required]
        public string UserCity { get; set; }

        [StringLength(
            GlobalConstants.Order.NeighborhoodMaxLength,
            MinimumLength = GlobalConstants.Order.NeighborhoodMinLength,
            ErrorMessage = GlobalConstants.Order.NeighborhoodError)]
        public string UserNeighborhood { get; set; }

        [StringLength(
            GlobalConstants.Order.StreetMaxLength,
            MinimumLength = GlobalConstants.Order.StreetMinLength,
            ErrorMessage = GlobalConstants.Order.StreetError)]
        public string UserStreet { get; set; }

        [Range(GlobalConstants.Order.StreetNumberMin,
            GlobalConstants.Order.StreetNumberMax,
            ErrorMessage = GlobalConstants.Order.StreetNumberError)]
        public int? UserStreetNumber { get; set; }

        [StringLength(
            GlobalConstants.Order.ApartmentBuildingMaxLength,
            MinimumLength = GlobalConstants.Order.ApartmentBuildingMinLength,
            ErrorMessage = GlobalConstants.Order.ApartmentBuildingError)]
        public string UserApartmentBuilding { get; set; }

        [StringLength(
            GlobalConstants.Order.EntranceMaxLength,
            MinimumLength = GlobalConstants.Order.EntranceMinLength,
            ErrorMessage = GlobalConstants.Order.EntranceError)]
        public string UserEntrance { get; set; }

        [Range(GlobalConstants.Order.FloorMin,
            GlobalConstants.Order.FloorMax,
            ErrorMessage = GlobalConstants.Order.FloorError)]
        public int? UserFloor { get; set; }

        [Range(GlobalConstants.Order.ApartmentNumberMin,
            GlobalConstants.Order.ApartmentNumberMax,
            ErrorMessage = GlobalConstants.Order.ApartmentNumberError)]
        public int? UserApartmentNumber { get; set; }

        [StringLength(
            GlobalConstants.Order.NotesMaxLength,
            MinimumLength = GlobalConstants.Order.NotesMinLength,
            ErrorMessage = GlobalConstants.Order.NotesError)]
        public string Notes { get; set; }

        public string OfficeName { get; set; }

        [Required]
        public DeliveryType DeliveryType { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string UserId { get; set; }

        public OfficesViewModel Offices { get; set; }

        public CitiesViewModel Cities { get; set; }
    }
}
