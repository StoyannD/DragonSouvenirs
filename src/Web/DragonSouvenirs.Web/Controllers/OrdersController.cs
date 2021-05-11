namespace DragonSouvenirs.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Models.Enums;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderService orderService;
        private readonly ICartService cartService;
        private readonly IProductsService productsService;

        public OrdersController(
            UserManager<ApplicationUser> userManager,
            IOrderService orderService,
            ICartService cartService,
            IProductsService productsService)
        {
            this.userManager = userManager;
            this.orderService = orderService;
            this.cartService = cartService;
            this.productsService = productsService;
        }

        public async Task<ActionResult> Create()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!await this.cartService.UserHasProductsInCart(user.Id))
            {
                this.TempData["fail"] = GlobalConstants.Order.EmptyCart;
                return this.RedirectToAction("Index", "Home");
            }

            var viewModel = new CreateOrderViewModel
            {
                ShippingAddress = user.DefaultShippingAddress,
                UserFullName = user.FullName,
                UserEmail = user.Email,
                InvoiceNumber = user.PhoneNumber,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<ActionResult> CreatePost(CreateOrderViewModel inputModel)
        {
            if (inputModel == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            inputModel.UserId = user.Id;
            inputModel.ExpectedDeliveryDate = DateTime.UtcNow.AddDays(3);
            inputModel.DeliveryPrice = GlobalConstants.Order.DeliveryPrice;

            await this.orderService.CreateOrderAsync(inputModel);

            return this.RedirectToAction(nameof(this.Summary));
        }

        public async Task<ActionResult> Summary()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = await this.orderService.GetProcessingOrderAsync<ConfirmOrderViewModel>(user.Id);

            var cartTotalPrice = await this.cartService.GetCartTotalPriceAsync(user.Id);
            viewModel.CartTotalPrice = cartTotalPrice;

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Summary")]
        public async Task<ActionResult> SummaryPost()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            await this.orderService.ConfirmOrderAsync(user.Id);

            this.TempData["success"] = GlobalConstants.Order.OrderCreated;

            return this.RedirectToAction("Index", "Home");
        }
    }
}
