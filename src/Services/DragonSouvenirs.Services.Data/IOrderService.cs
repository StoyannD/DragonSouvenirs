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
    }
}
