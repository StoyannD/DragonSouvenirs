using DragonSouvenirs.Data.Models.Enums;

namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

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
