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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderService orderService;

        public OrderProductsComponent(
            UserManager<ApplicationUser> userManager,
            IOrderService orderService)
        {
            this.userManager = userManager;
            this.orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId, int orderId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            var orderProducts = await this.orderService
                .GetOrderProductsAsync<OrderProductsViewModel>(userId, orderId);

            return this.View(orderProducts);
        }
    }
}
