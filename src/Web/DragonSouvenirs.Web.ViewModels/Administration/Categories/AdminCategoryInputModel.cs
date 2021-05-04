﻿namespace DragonSouvenirs.Web.ViewModels.Administration.Categories
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class AdminCategoryInputModel : IMapTo<Category>
    {
        [Required]
        [StringLength(
            GlobalConstants.Category.NameMaxLength,
            MinimumLength = GlobalConstants.Category.NameMinLength,
            ErrorMessage = GlobalConstants.Category.NameLengthError)]
        public string Name { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.Category.TitleMaxLength,
            MinimumLength = GlobalConstants.Category.TitleMinLength,
            ErrorMessage = GlobalConstants.Category.TitleLengthError)]
        public string Title { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.Category.ContentMaxLength,
            MinimumLength = GlobalConstants.Category.ContentMinLength,
            ErrorMessage = GlobalConstants.Category.ContentLengthError)]
        public string Content { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
