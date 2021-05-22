namespace DragonSouvenirs.Web.ViewModels.Components.CartBasketComponent
{
    using System.Linq;

    using AutoMapper;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CartBasketViewModel : IMapFrom<Cart>, IHaveCustomMappings
    {
        public int CartProductsCount { get; set; }

        public decimal CartTotal { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Cart, CartBasketViewModel>()
                .ForMember(m => m.CartProductsCount, options =>
                {
                    options.MapFrom(
                        p => p.CartProducts.Sum(cp => cp.Quantity));
                })
                .ForMember(m => m.CartTotal, options =>
                {
                    options.MapFrom(
                        p => p.CartProducts
                            .Sum(cp => cp.Quantity * (cp.Product.DiscountPrice ?? cp.Product.Price)));
                });
        }
    }
}
