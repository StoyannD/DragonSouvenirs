namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Models.Enums;
    using DragonSouvenirs.Web.ViewModels.Orders;

    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderViewModel model);

        Task<T> GetProcessingOrderAsync<T>(string userId);

        Task ConfirmOrderAsync(string userId);

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetCompletedAsync<T>();

        Task<OrderStatus> GetOrderStatusAsync(int orderId);

        Task<T> GetAdminOrderDetailsAsync<T>(int? id);

        Task<IEnumerable<T>> GetOrderProductsAsync<T>(string userId, int orderId);

        Task ProcessOrderAsync(int orderId, int orderStatus);
    }
}
