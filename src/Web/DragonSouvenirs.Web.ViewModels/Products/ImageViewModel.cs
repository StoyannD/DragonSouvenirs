namespace DragonSouvenirs.Web.ViewModels.Products
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class ImageViewModel : IMapFrom<Image>
    {
        public string ImgUrl { get; set; }
    }
}
