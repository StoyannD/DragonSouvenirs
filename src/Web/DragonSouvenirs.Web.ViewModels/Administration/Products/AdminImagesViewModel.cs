namespace DragonSouvenirs.Web.ViewModels.Administration.Products
{
    using System.ComponentModel.DataAnnotations;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class AdminImagesViewModel : IMapFrom<Image>
    {
        [Url]
        public string ImgUrl { get; set; }

        public int Id { get; set; }
    }
}
