using DragonSouvenirs.Web.ViewModels.Products;

namespace DragonSouvenirs.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.Helpers;
    using DragonSouvenirs.Web.ViewModels.Cart;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IProductsService productsService;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(
            ICartService cartService,
            IProductsService productsService,
            UserManager<ApplicationUser> userManager)
        {
            this.cartService = cartService;
            this.productsService = productsService;
            this.userManager = userManager;
        }

        [Route(
            "/Cart",
            Name = "cartRoute")]
        public async Task<ActionResult> Index()
        {
            var viewModel = new CartAllProductsViewModel();
            if (this.User.Identity.IsAuthenticated)
            {
                var user = await this.userManager.GetUserAsync(this.User);

                var cartProducts = await this.cartService
                    .GetCartProductsAsync<CartProductViewModel>(user.Id);
                viewModel.CartProducts = cartProducts;
                return this.View(viewModel);
            }

            var cartSession = this.GetSessionCart();
            viewModel.CartProducts = cartSession;
            return this.View(viewModel);
        }

        public async Task<ActionResult> Add(int id)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var user = await this.userManager.GetUserAsync(this.User);
                await this.cartService.AddProductToCartAsync(user.Id, id);
            }
            else
            {
                // TODO: GuestCartAdd
            }

            return this.RedirectToAction("Index", "Home", new { id = id });
        }

        public async Task<ActionResult> Remove(int id)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var user = await this.userManager.GetUserAsync(this.User);
                await this.cartService.DeleteProductFromCartAsync(user.Id, id);
            }
            else
            {
                // TODO: GuestCartRemove
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        private IEnumerable<CartProductViewModel> GetSessionCart()
        {
            return this.HttpContext
                       .Session
                       .GetObjectFromJson<CartProductViewModel[]>(GlobalConstants.Sessions.CartSessionKey)
                   ?? new List<CartProductViewModel>().ToArray();
        }
    }
}
