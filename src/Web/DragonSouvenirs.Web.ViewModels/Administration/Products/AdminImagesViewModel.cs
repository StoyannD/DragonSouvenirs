namespace DragonSouvenirs.Web.ViewModels.Administration.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class AdminImagesViewModel : IMapFrom<Image>
    {
        [Url]
        public string ImgUrl { get; set; }

        public int Id { get; set; }
    }
}
