namespace DragonSouvenirs.Web.ViewModels.Administration.Users
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class ApplicationUserEditViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        [Display(Name = GlobalConstants.User.UserNameDisplay)]
        [Required]
        [StringLength(
            GlobalConstants.User.UserNameMaxLength,
            MinimumLength = GlobalConstants.User.UserNameMinLength,
            ErrorMessage = GlobalConstants.User.UserNameLengthError)]
        public string UserName { get; set; }

        [Display(Name = GlobalConstants.User.FullNameDisplay)]
        [Required]
        [StringLength(
            GlobalConstants.User.FullNameMaxLength,
            MinimumLength = GlobalConstants.User.FullNameMinLength,
            ErrorMessage = GlobalConstants.User.FullNameLengthError)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = GlobalConstants.User.EmailAddressError)]
        public string Email { get; set; }

        public string DefaultShippingAddress { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.User.CityMaxLength,
            MinimumLength = GlobalConstants.User.CityMinLength,
            ErrorMessage = GlobalConstants.User.CityError)]
        public string City { get; set; }

        // [Required]
        [StringLength(
            GlobalConstants.User.NeighborhoodMaxLength,
            MinimumLength = GlobalConstants.User.NeighborhoodMinLength,
            ErrorMessage = GlobalConstants.User.NeighborhoodError)]
        public string Neighborhood { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.User.StreetMaxLength,
            MinimumLength = GlobalConstants.User.StreetMinLength,
            ErrorMessage = GlobalConstants.User.StreetError)]
        public string Street { get; set; }

        [Required]
        [Range(GlobalConstants.User.StreetNumberMin,
            GlobalConstants.User.StreetNumberMax,
            ErrorMessage = GlobalConstants.User.StreetNumberError)]
        public int StreetNumber { get; set; }

        [StringLength(
            GlobalConstants.User.ApartmentBuildingMaxLength,
            MinimumLength = GlobalConstants.User.ApartmentBuildingMinLength,
            ErrorMessage = GlobalConstants.User.ApartmentBuildingError)]
        public string ApartmentBuilding { get; set; }

        [StringLength(
            GlobalConstants.User.EntranceMaxLength,
            MinimumLength = GlobalConstants.User.EntranceMinLength,
            ErrorMessage = GlobalConstants.User.EntranceError)]
        public string Entrance { get; set; }

        [Required]
        [Range(GlobalConstants.User.FloorMin,
            GlobalConstants.User.FloorMax,
            ErrorMessage = GlobalConstants.User.FloorError)]
        public int Floor { get; set; }

        [Required]
        [Range(GlobalConstants.User.ApartmentNumberMin,
            GlobalConstants.User.ApartmentNumberMax,
            ErrorMessage = GlobalConstants.User.ApartmentNumberError)]
        public int ApartmentNumber { get; set; }

        public int PersonalDiscountPercentage { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
