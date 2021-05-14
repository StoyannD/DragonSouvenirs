namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Models.Enums;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels.Orders;
    using Microsoft.EntityFrameworkCore;

    public class OrderService : IOrderService
    {
        private readonly IDeletableEntityRepository<Order> orderRepository;
        private readonly IDeletableEntityRepository<OrderProduct> orderProductRepository;
        private readonly IDeletableEntityRepository<CartProduct> cartProductRepository;
        private readonly IDeletableEntityRepository<Cart> cartRepository;

        public OrderService(
            IDeletableEntityRepository<Order> orderRepository,
            IDeletableEntityRepository<OrderProduct> orderProductRepository,
            IDeletableEntityRepository<CartProduct> cartProductRepository,
            IDeletableEntityRepository<Cart> cartRepository)
        {
            this.orderRepository = orderRepository;
            this.orderProductRepository = orderProductRepository;
            this.cartProductRepository = cartProductRepository;
            this.cartRepository = cartRepository;
        }

        public async Task CreateOrderAsync(CreateOrderViewModel model)
        {
            if (!await this.orderRepository
                .All()
                .AnyAsync(o => o.OrderStatus == OrderStatus.Created))
            {
                var order = new Order()
                {
                    CreatedOn = DateTime.UtcNow,
                    UserId = model.UserId,
                    OrderStatus = OrderStatus.Created,
                    ExpectedDeliveryDate = model.ExpectedDeliveryDate,
                    ShippingAddress = model.ShippingAddress,
                    DeliveryPrice = model.DeliveryPrice,
                    TotalPrice = model.TotalPrice,
                    UserEmail = model.UserEmail,
                    InvoiceNumber = model.InvoiceNumber,
                    UserFullName = model.UserFullName,
                };

                await this.orderRepository.AddAsync(order);
                await this.orderRepository.SaveChangesAsync();
            }
        }

        public async Task<T> GetProcessingOrderAsync<T>(string userId)
        {
            var order = await this.orderRepository
                .All()
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .Where(x => x.UserId == userId && x.OrderStatus == OrderStatus.Created)
                .To<T>()
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task ConfirmOrderAsync(string userId)
        {
            var order = await this.orderRepository
                .All()
                .FirstOrDefaultAsync(x => x.UserId == userId
                                          && x.OrderStatus == OrderStatus.Created);

            var cartProducts = await this.cartProductRepository
                .All()
                .Include(cp => cp.Product)
                .Where(cp => cp.Cart.UserId == userId)
                .ToListAsync();

            var orderProducts = cartProducts
                .Select(x => new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Product.Price,
                })
                .ToList();

            foreach (var orderProduct in orderProducts)
            {
                await this.orderProductRepository.AddAsync(orderProduct);
            }

            await this.orderProductRepository.SaveChangesAsync();

            order.TotalPrice = orderProducts.Sum(op => op.Quantity * op.Price);
            order.OrderStatus = OrderStatus.Processing;
            await this.orderRepository.SaveChangesAsync();

            // Empty user cart
            foreach (var cartProduct in cartProducts)
            {
                this.cartProductRepository.HardDelete(cartProduct);
            }

            var cart = await this.cartRepository
                .All()
                .FirstOrDefaultAsync(c => c.UserId == userId);
            this.cartRepository.HardDelete(cart);
            await this.cartProductRepository.SaveChangesAsync();
            await this.cartRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAdminAsync<T>()
        {
            var orders = await this.orderRepository
                .AllWithDeleted()
                .OrderByDescending(o => o.CreatedOn)
                .ThenByDescending(o => o.TotalPrice)
                .To<T>()
                .ToListAsync();

            return orders;
        }

        public async Task<T> GetAdminOrderDetailsAsync<T>(int? id)
        {
            var order = await this.orderRepository
                .AllWithDeleted()
                .Where(o => o.Id == id.Value)
                .To<T>()
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<IEnumerable<T>> GetOrderProductsAsync<T>(string userId, int orderId)
        {
            var orderProducts = await this.orderProductRepository
                .AllWithDeleted()
                .Where(op => op.OrderId == orderId &&
                             op.Order.UserId == userId)
                .To<T>()
                .ToListAsync();

            return orderProducts;
        }

        public async Task ProcessOrderAsync(int orderId, int orderStatus)
        {
            var order = await this.orderRepository
                .All()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            order.OrderStatus = (OrderStatus)orderStatus;
            await this.orderRepository.SaveChangesAsync();
        }
    }
}
