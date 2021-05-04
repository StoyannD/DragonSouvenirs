namespace DragonSouvenirs.Web.ViewModels.Administration.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using AutoMapper;
    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class AdminProductInputModel : IMapTo<Product>
    {
        public AdminProductInputModel()
        {
            this.Images = new List<AdminImagesViewModel>();
            for (var i = 0; i < GlobalConstants.Image.ImagesPerProduct; i++)
            {
                this.Images.Add(new AdminImagesViewModel());
            }
        }

        public int Id { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.Product.NameMaxLength,
            MinimumLength = GlobalConstants.Product.NameMinLength,
            ErrorMessage = GlobalConstants.Product.NameLengthError)]
        public string Name { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.Product.TitleMaxLength,
            MinimumLength = GlobalConstants.Product.TitleMinLength,
            ErrorMessage = GlobalConstants.Product.TitleLengthError)]
        public string Title { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.Product.DescriptionMaxLength,
            MinimumLength = GlobalConstants.Product.DescriptionMinLength,
            ErrorMessage = GlobalConstants.Product.DescriptionLengthError)]
        public string Description { get; set; }

        [Required]
        [Range(
            (double)GlobalConstants.Product.PriceMin,
            (double)GlobalConstants.Product.PriceMax,
            ErrorMessage = GlobalConstants.Product.PriceInRangeError)]
        public decimal Price { get; set; }

        [Required]
        [Range(
            GlobalConstants.Product.QuantityMin,
            GlobalConstants.Product.QuantityMax,
            ErrorMessage = GlobalConstants.Product.QuantityInRangeError)]
        public int Quantity { get; set; }

        [Required]
        [Range(
            GlobalConstants.Product.SizeMin,
            GlobalConstants.Product.SizeMax,
            ErrorMessage = GlobalConstants.Product.SizeInRangeError)]
        public int Height { get; set; }

        [Required]
        [Range(
            GlobalConstants.Product.SizeMin,
            GlobalConstants.Product.SizeMax,
            ErrorMessage = GlobalConstants.Product.SizeInRangeError)]
        public int Width { get; set; }

        [Required]
        public List<CategoriesViewModel> Categories { get; set; }

        public IEnumerable<CategoriesDropdownViewModel> AllCategoriesDropdown { get; set; }

        [Required]
        public List<AdminImagesViewModel> Images { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Url]
        public string UrlValidation { get; set; }
    }
}
