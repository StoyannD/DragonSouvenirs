namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Common.Repositories;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Models.Enums;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Services.Messaging;
    using DragonSouvenirs.Web.ViewModels.Orders;
    using Microsoft.EntityFrameworkCore;

    public class OrderService : IOrderService
    {
        private readonly IDeletableEntityRepository<Order> orderRepository;
        private readonly IDeletableEntityRepository<OrderProduct> orderProductRepository;
        private readonly IDeletableEntityRepository<CartProduct> cartProductRepository;
        private readonly IDeletableEntityRepository<Cart> cartRepository;
        private readonly IDeletableEntityRepository<Product> productRepository;
        private readonly IEmailSender emailSender;
        private readonly IEmailTemplatesSender emailTemplatesService;
        private readonly ICommonFeaturesService commonFeaturesService;

        public OrderService(
            IDeletableEntityRepository<Order> orderRepository,
            IDeletableEntityRepository<OrderProduct> orderProductRepository,
            IDeletableEntityRepository<CartProduct> cartProductRepository,
            IDeletableEntityRepository<Cart> cartRepository,
            IDeletableEntityRepository<Product> productRepository,
            IEmailSender emailSender,
            IEmailTemplatesSender emailTemplatesService,
            ICommonFeaturesService commonFeaturesService)
        {
            this.orderRepository = orderRepository;
            this.orderProductRepository = orderProductRepository;
            this.cartProductRepository = cartProductRepository;
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.emailSender = emailSender;
            this.emailTemplatesService = emailTemplatesService;
            this.commonFeaturesService = commonFeaturesService;
        }

        // Get all Orders of a user by userId
        public async Task<IEnumerable<T>> GetAllByUserIdAsync<T>(string userId)
        {
            var orders = await this.orderRepository
                .AllWithDeleted()
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedOn)
                .ThenByDescending(o => o.TotalPrice)
                .To<T>()
                .ToListAsync();

            return orders;
        }

        // Create initial order (Step 1)
        public async Task CreateOrderAsync(CreateOrderViewModel model)
        {
            if (!await this.orderRepository
                .All()
                .AnyAsync(o => o.OrderStatus == OrderStatus.Created))
            {
                var userFullName = $"{model.FirstName} {model.LastName}";
                var officeBrand = model.OfficeBrand;

                var shippingAddress = model.DeliveryType == DeliveryType.ToAddress
                    ? this.commonFeaturesService
                        .RenderAddress(
                            model.UserCity,
                            model.UserNeighborhood,
                            model.UserStreet,
                            model.UserStreetNumber,
                            model.UserApartmentBuilding,
                            model.UserEntrance,
                            model.UserFloor,
                            model.UserApartmentNumber)
                    : model.OfficeName;

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

        // Get processing order of the user (Step 2)
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

        // Confirm the order (Step 3)
        public async Task ConfirmOrderAsync(string userId, decimal personalDiscountPercentage)
        {
            // [1] Get the user's order
            var order = await this.orderRepository
                .All()
                .FirstOrDefaultAsync(x => x.UserId == userId
                                          && x.OrderStatus == OrderStatus.Created);

            // [2] Get the CartProducts of user's shopping cart
            var cartProducts = await this.cartProductRepository
                .All()
                .Include(cp => cp.Product)
                .Where(cp => cp.Cart.UserId == userId)
                .ToListAsync();

            // [3] Create OrderProducts from CartProducts
            var orderProducts = cartProducts
                .Select(x => new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Product.DiscountPrice ?? x.Product.Price,
                })
                .ToList();

            // [4] Persist the OrderProducts to the user's order
            foreach (var orderProduct in orderProducts)
            {
                await this.orderProductRepository.AddAsync(orderProduct);
            }

            await this.orderProductRepository.SaveChangesAsync();

            // [5] Calculate the total price of the order, including the personal discount
            order.TotalPrice = orderProducts.Sum(op => op.Quantity * op.Price);
            order.TotalPrice *= personalDiscountPercentage == 0
                ? 1
                : 1 - (personalDiscountPercentage / 100);

            // [6] Reduce the quantity of products in stock
            foreach (var cartProduct in cartProducts)
            {
                var product = await this.productRepository.All().FirstOrDefaultAsync(p => p.Id == cartProduct.ProductId);
                if (product.Quantity < cartProduct.Quantity)
                {
                    throw new ArgumentOutOfRangeException();
                }

                product.Quantity -= cartProduct.Quantity;

                // [7] Delete the CartProducts in user's shopping cart
                this.cartProductRepository.HardDelete(cartProduct);
            }

            // [8] Delete the user's shopping cart
            var cart = await this.cartRepository
                .All()
                .FirstOrDefaultAsync(c => c.UserId == userId);
            this.cartRepository.HardDelete(cart);

            // [9] Change the order status and persist the changes in the database
            order.OrderStatus = OrderStatus.Processing;

            // [10] Persist the changes to the database
            await this.productRepository.SaveChangesAsync();
            await this.cartProductRepository.SaveChangesAsync();
            await this.cartRepository.SaveChangesAsync();
            await this.orderRepository.SaveChangesAsync();

            // [11] Send Email via SendGrid API
            await this.SendEmailAsync(
                GlobalConstants.EmailTemplates.SubmitOrder.Title,
                GlobalConstants.EmailTemplates.SubmitOrder.Subject,
                order);
        }

        // Get all orders, ordered by date and total price
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

        // Get COMPLETED orders(marked as deleted),
        // ordered by date of delivery and total price
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

        // Get the status of an order by orderId
        public async Task<OrderStatus> GetOrderStatusAsync(int orderId)
        {
            var order = await this.orderRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return order.OrderStatus;
        }

        // Get order from All WITH DELETED, by Id
        public async Task<T> GetAdminOrderDetailsAsync<T>(int? id)
        {
            var order = await this.orderRepository
                .AllWithDeleted()
                .Where(o => o.Id == id.Value)
                .To<T>()
                .FirstOrDefaultAsync();

            return order;
        }

        // Get the products in a user's order by orderId
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

        // Process the order status
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

            // Send Email on order completion
            if (updatedOrderStatus == OrderStatus.Completed)
            {
                order.IsDeleted = true;
                order.DateOfDelivery = DateTime.UtcNow;

                await this.SendEmailAsync(
                    GlobalConstants.EmailTemplates.CompleteOrder.Title,
                    GlobalConstants.EmailTemplates.CompleteOrder.Subject,
                    order);
            }

            // Send Email on order acceptance
            if (updatedOrderStatus == OrderStatus.Accepted)
            {
                await this.SendEmailAsync(
                    GlobalConstants.EmailTemplates.AcceptOrder.Title,
                    GlobalConstants.EmailTemplates.AcceptOrder.Subject,
                    order);
            }

            order.OrderStatus = updatedOrderStatus;
            await this.orderRepository.SaveChangesAsync();
        }

        private async Task SendEmailAsync(string orderTitle, string emailSubject, Order order)
        {
            // Render Email template
            var emailTemplate = this.emailTemplatesService.Order(orderTitle,
                order.ShippingAddress,
                order.ClientFullName,
                order.InvoiceNumber,
                order.TotalPrice,
                order.OrderProducts,
                order.DeliveryPrice);

            // Send Email via SendGrid API
            await this.emailSender.SendEmailAsync(
                GlobalConstants.EmailTemplates.SenderEmail,
                GlobalConstants.EmailTemplates.WebsiteName,
                order.UserEmail,
                emailSubject,
                emailTemplate);
        }
    }
}
