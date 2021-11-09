namespace DragonSouvenirs.Web.Controllers
{
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Cart;
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

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
