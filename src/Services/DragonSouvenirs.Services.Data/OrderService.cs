namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
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
        private readonly IDeletableEntityRepository<Product> productRepository;

        public OrderService(
            IDeletableEntityRepository<Order> orderRepository,
            IDeletableEntityRepository<OrderProduct> orderProductRepository,
            IDeletableEntityRepository<CartProduct> cartProductRepository,
            IDeletableEntityRepository<Cart> cartRepository,
            IDeletableEntityRepository<Product> productRepository)
        {
            this.orderRepository = orderRepository;
            this.orderProductRepository = orderProductRepository;
            this.cartProductRepository = cartProductRepository;
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
        }

        public async Task<IEnumerable<T>> GetAllByUserIdAsync<T>(string userId)
        {
            var orders = await this.orderRepository
                .All()
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedOn)
                .ThenByDescending(o => o.TotalPrice)
                .To<T>()
                .ToListAsync();

            return orders;
        }

        public async Task CreateOrderAsync(CreateOrderViewModel model)
        {
            if (!await this.orderRepository
                .All()
                .AnyAsync(o => o.OrderStatus == OrderStatus.Created))
            {
                var userFullName = model.FirstName + " " + model.LastName;
                string shippingAddress;
                OfficeBrands officeBrand = model.OfficeBrand; ;
                if (model.DeliveryType == DeliveryType.ToAddress)
                {
                    shippingAddress = "гр. " + model.UserCity + ", кв. "
                                      + model.UserNeighborhood
                                      + ", ул. "
                                      + model.UserStreet
                                      + " "
                                      + model.UserStreetNumber;

                    shippingAddress += model.UserApartmentBuilding != null
                        ? ", бл. " + model.UserApartmentBuilding + " " : string.Empty;
                    shippingAddress += model.UserEntrance != null
                        ? ", вх. " + model.UserEntrance + " " : string.Empty;

                    shippingAddress += ", ет. "
                                       + model.UserFloor
                                       + ", ап. "
                                       + model.UserApartmentNumber;
                }
                else
                {
                    shippingAddress = model.OfficeName;
                }

                var order = new Order()
                {
                    CreatedOn = DateTime.UtcNow,
                    UserId = model.UserId,
                    OrderStatus = OrderStatus.Created,
                    DeliveryType = model.DeliveryType,
                    ExpectedDeliveryDate = model.ExpectedDeliveryDate,
                    ShippingAddress = shippingAddress,
                    DeliveryPrice = model.DeliveryPrice,
                    TotalPrice = model.TotalPrice,
                    UserEmail = model.UserEmail,
                    InvoiceNumber = model.InvoiceNumber,
                    ClientFullName = userFullName,
                    Notes = model.Notes,
                    OfficeBrand = officeBrand,
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
                    Price = x.Product.DiscountPrice ?? x.Product.Price,
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

            // Reduce quantity of products
            // Empty user cart
            foreach (var cartProduct in cartProducts)
            {
                var product = await this.productRepository.All().FirstOrDefaultAsync(p => p.Id == cartProduct.ProductId);
                if (product.Quantity < cartProduct.Quantity)
                {
                    throw new ArgumentOutOfRangeException();
                }

                product.Quantity -= cartProduct.Quantity;
                this.cartProductRepository.HardDelete(cartProduct);
            }

            var cart = await this.cartRepository
                .All()
                .FirstOrDefaultAsync(c => c.UserId == userId);
            this.cartRepository.HardDelete(cart);

            await this.productRepository.SaveChangesAsync();
            await this.cartProductRepository.SaveChangesAsync();
            await this.cartRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var orders = await this.orderRepository
                .All()
                .OrderByDescending(o => o.CreatedOn)
                .ThenByDescending(o => o.TotalPrice)
                .To<T>()
                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<T>> GetCompletedAsync<T>()
        {
            var orders = await this.orderRepository
                .AllWithDeleted()
                .Where(o => o.IsDeleted)
                .OrderByDescending(o => o.DateOfDelivery)
                .ThenByDescending(o => o.TotalPrice)
                .To<T>()
                .ToListAsync();

            return orders;
        }

        public async Task<OrderStatus> GetOrderStatusAsync(int orderId)
        {
            var order = await this.orderRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return order.OrderStatus;
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
                .AllWithDeleted()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            var updatedOrderStatus = (OrderStatus)orderStatus;
            if (order.OrderStatus == OrderStatus.Completed)
            {
                order.DateOfDelivery = null;
                order.IsDeleted = false;
            }

            if (updatedOrderStatus == OrderStatus.Completed)
            {
                order.IsDeleted = true;
                order.DateOfDelivery = DateTime.UtcNow;
            }

            order.OrderStatus = updatedOrderStatus;

            await this.orderRepository.SaveChangesAsync();
        }
    }
}
