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

        [Display(Name = GlobalConstants.User.DefaultShippingAddressDisplay)]
        [Required]
        [StringLength(
            GlobalConstants.User.DefaultShippingAddressMaxLength,
            MinimumLength = GlobalConstants.User.DefaultShippingAddressMinLength,
            ErrorMessage = GlobalConstants.User.DefaultShippingAddressLengthError)]
        public string DefaultShippingAddress { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
