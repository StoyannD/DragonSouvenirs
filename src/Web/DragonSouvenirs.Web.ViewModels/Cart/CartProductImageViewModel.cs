namespace DragonSouvenirs.Web.ViewModels.Cart
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CartProductImageViewModel : IMapFrom<Image>
    {
        public string ImgUrl { get; set; }
    }
}
