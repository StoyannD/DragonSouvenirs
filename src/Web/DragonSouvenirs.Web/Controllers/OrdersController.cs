using DragonSouvenirs.Data.Models;
using DragonSouvenirs.Data.Models.Enums;
using DragonSouvenirs.Services.Data;
using DragonSouvenirs.Web.ViewModels.Orders;
using Microsoft.AspNetCore.Identity;

namespace DragonSouvenirs.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderService orderService;
        private readonly ICartService cartService;

        public OrdersController(
            UserManager<ApplicationUser> userManager,
            IOrderService orderService,
            ICartService cartService)
        {
            this.userManager = userManager;
            this.orderService = orderService;
            this.cartService = cartService;
        }

        public async Task<ActionResult> Create()
        {
            // TODO: Check if cart is empty.
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = new CreateOrderViewModel
            {
                UserDefaultShippingAddress = user.DefaultShippingAddress,
                UserUserName = user.UserName,
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
            inputModel.DeliveryPrice = 10M;

            await this.orderService.CreateOrderAsync(inputModel);

            return this.RedirectToAction(nameof(this.OrderSummary));
        }

        public async Task<ActionResult> OrderSummary()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = await this.orderService.GetProcessingOrderAsync<ConfirmOrderViewModel>(user.Id);

            var cartTotalPrice = await this.cartService.GetCartTotalPriceAsync(user.Id);
            viewModel.CartTotalPrice = cartTotalPrice;

            return this.View(viewModel);
        }

        public async Task<ActionResult> ConfirmOrder()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            await this.orderService.ConfirmOrderAsync(user.Id);

            this.TempData["success"] = "Order Created Successfully!";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
