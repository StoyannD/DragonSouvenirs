namespace DragonSouvenirs.Web.Components
{
    using System.Threading.Tasks;

    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Components.OrderProductsComponent;
    using Microsoft.AspNetCore.Mvc;

    public class OrderProductsComponent : ViewComponent
    {
        private readonly IOrderService orderService;

        public OrderProductsComponent(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId, int orderId)
        {
            var orderProducts = await this.orderService
                .GetOrderProductsAsync<OrderProductsViewModel>(userId, orderId);

            return this.View(orderProducts);
        }
    }
}
