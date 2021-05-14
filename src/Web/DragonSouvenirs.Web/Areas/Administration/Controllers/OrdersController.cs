namespace DragonSouvenirs.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper.Configuration.Annotations;
    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Models.Enums;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.Controllers;
    using DragonSouvenirs.Web.ViewModels.Administration.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<ActionResult> Index()
        {
            var viewModel = new AllOrdersViewModel();
            var orders = await this.orderService.GetAllAdminAsync<OrderViewModel>();
            viewModel.Orders = orders;

            this.TempData.Keep();

            return this.View(viewModel);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var viewModel = await this.orderService.GetAdminOrderDetailsAsync<OrderDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus(int orderId, OrderStatus orderStatus)
        {
            await this.orderService.ProcessOrderAsync(orderId, (int)orderStatus);

            this.TempData["success"] = "Order updated successfully!";
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
