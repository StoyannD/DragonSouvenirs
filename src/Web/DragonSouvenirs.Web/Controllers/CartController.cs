namespace DragonSouvenirs.Web.Controllers
{
    using System.Collections.Generic;
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

        public async Task<ActionResult> Add(int? id, bool toCart, int? quantity)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            if (this.User.Identity.IsAuthenticated)
            {
                var user = await this.userManager.GetUserAsync(this.User);
                await this.cartService.AddProductToCartAsync(user.Id, id.Value, quantity ?? 1);
            }
            else
            {
                // TODO: GuestCartAdd
            }

            if (toCart)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            if (this.User.Identity.IsAuthenticated)
            {
                var user = await this.userManager.GetUserAsync(this.User);
                await this.cartService.DeleteProductFromCartAsync(user.Id, id.Value);
            }
            else
            {
                // TODO: GuestCartRemove
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int? id, int? quantity)
        {
            if (id == null || quantity == null)
            {
                return this.BadRequest();
            }

            if (quantity < 0)
            {
                return this.BadRequest();
            }

            if (this.User.Identity.IsAuthenticated)
            {
                var user = await this.userManager.GetUserAsync(this.User);
                await this.cartService
                    .EditProductInCartAsync(user.Id, id.Value, quantity.Value);
            }
            else
            {
                // TODO: GuestCartEdit
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
