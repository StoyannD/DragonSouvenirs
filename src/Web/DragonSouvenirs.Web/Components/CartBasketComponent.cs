namespace DragonSouvenirs.Web.Components
{
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Components.CartBasketComponent;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CartBasketComponent : ViewComponent
    {
        private readonly ICartService cartService;
        private readonly IProductsService productsService;
        private readonly UserManager<ApplicationUser> userManager;

        public CartBasketComponent(
            ICartService cartService,
            IProductsService productsService,
            UserManager<ApplicationUser> userManager)
        {
            this.cartService = cartService;
            this.productsService = productsService;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync(this.UserClaimsPrincipal);

            // TODO:add favorite products
            var viewModel = new ComponentViewModel();
            if (user != null)
            {
                viewModel.FavouriteProducts = await this.productsService
                    .GetFavouriteProductsAsync<FavouriteProductViewModel>(user.Id);
                viewModel.Cart = await this.cartService.GetCartByIdAsync<CartBasketViewModel>(user.Id);
            }

            return this.View(viewModel);
        }
    }
}
