namespace DragonSouvenirs.Web.ViewModels.Administration.Products
{
    using System.ComponentModel.DataAnnotations;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class AdminImagesViewModel : IMapFrom<Image>
    {
        [Url]
        public string ImgUrl { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        public int Id { get; set; }

        public bool ToDelete { get; set; }
    }
}
