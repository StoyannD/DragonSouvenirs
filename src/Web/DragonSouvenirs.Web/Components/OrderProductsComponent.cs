namespace DragonSouvenirs.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Components.OrderProductsComponent;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class OrderProductsComponent : ViewComponent
    {
        private readonly ICartService cartService;
        private readonly UserManager<ApplicationUser> userManager;

        public OrderProductsComponent(
            ICartService cartService,
            UserManager<ApplicationUser> userManager)
        {
            this.cartService = cartService;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name)
        {
            var user = await this.userManager.GetUserAsync(this.UserClaimsPrincipal);

            var cartProducts = await this.cartService.GetCartProductsAsync<CartProductsViewModel>(user.Id);

            return this.View(cartProducts);
        }
    }
}
