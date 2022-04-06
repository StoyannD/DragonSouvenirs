namespace DragonSouvenirs.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models.Enums;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Administration.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class OrdersController : BaseAdminController
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<ActionResult> Index()
        {
            var viewModel = new AllOrdersViewModel();
            var orders = await this.orderService.GetAllAsync<OrderViewModel>();
            viewModel.Orders = orders;

            this.TempData.Keep();

            return this.View(viewModel);
        }

        public async Task<ActionResult> Completed()
        {
            var viewModel = new AllOrdersViewModel();
            var orders = await this.orderService.GetCompletedAsync<OrderViewModel>();
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
            var oldOrderStatus = await this.orderService
                .GetOrderStatusAsync(orderId);

            await this.orderService.ProcessOrderAsync(orderId, (int)orderStatus);

            this.TempData["success"] = GlobalConstants.Order.OrderUpdated;

            return this.RedirectToAction(oldOrderStatus == OrderStatus.Completed
                ? nameof(this.Completed)
                : nameof(this.Index));
        }
    }
}
