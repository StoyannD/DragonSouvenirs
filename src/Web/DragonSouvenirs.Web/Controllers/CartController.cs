namespace DragonSouvenirs.Web.Controllers
{
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Cart;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(
            ICartService cartService,
            UserManager<ApplicationUser> userManager)
        {
            this.cartService = cartService;
            this.userManager = userManager;
        }

        [Authorize]
        [Route(
            "/Cart",
            Name = GlobalConstants.Routes.CartRoute)]
        public async Task<ActionResult> Index()
        {
            var viewModel = new CartAllProductsViewModel();

            var user = await this.userManager.GetUserAsync(this.User);

            var cartProducts =
                await this.cartService.GetCartProductsAsync<CartProductViewModel>(user.Id);
            viewModel.CartProducts = cartProducts;

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<ActionResult> Add(int? id, bool toCart, int? quantity)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var success = await this.cartService
                .AddProductToCartAsync(user.Id, id.Value, quantity ?? 1);

            if (!success)
            {
                return this.BadRequest();
            }

            return toCart
                ? this.RedirectToAction(nameof(this.Index))
                : this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<ActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var success = await this.cartService.DeleteProductFromCartAsync(user.Id, id.Value);

            if (!success)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [Authorize]
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

            var user = await this.userManager.GetUserAsync(this.User);

            var success = await this.cartService
                .EditProductInCartAsync(user.Id, id.Value, quantity.Value);

            if (!success)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
