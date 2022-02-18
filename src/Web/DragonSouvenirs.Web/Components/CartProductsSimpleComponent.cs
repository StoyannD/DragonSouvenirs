namespace DragonSouvenirs.Web.Components
{
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Components.CartProductsSimpleComponent;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CartProductsSimpleComponent : ViewComponent
    {
        private readonly ICartService cartService;
        private readonly UserManager<ApplicationUser> userManager;

        public CartProductsSimpleComponent(
            ICartService cartService,
            UserManager<ApplicationUser> userManager)
        {
            this.cartService = cartService;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync(this.UserClaimsPrincipal);

            var viewModel = new CartViewModel
            {
                PersonalDiscountPercentage = user.PersonalDiscountPercentage,
                Products = await this.cartService.GetCartProductsAsync<CartProductsSimpleViewModel>(user.Id),
            };

            return this.View(viewModel);
        }
    }
}
