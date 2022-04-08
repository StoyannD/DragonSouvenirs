namespace DragonSouvenirs.Web.ViewModels.Cart
{
    using DragonSouvenirs.Services.Mapping;

    public class SimpleCartViewModel : IMapFrom<Data.Models.Cart>
    {
        public int Id { get; set; }
    }
}
