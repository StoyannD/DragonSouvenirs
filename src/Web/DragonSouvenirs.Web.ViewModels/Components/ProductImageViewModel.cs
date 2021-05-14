namespace DragonSouvenirs.Web.ViewModels.Components
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class ProductImageViewModel : IMapFrom<Image>
    {
        public string ImgUrl { get; set; }
    }
}
