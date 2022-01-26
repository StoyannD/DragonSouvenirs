namespace DragonSouvenirs.Services.Messaging
{
    using System.Collections.Generic;

    using DragonSouvenirs.Data.Models;

    public interface IEmailTemplatesSender
    {
        public string CreateOrder(string shippingAddress, string clientFullName, string invoiceNumber,
            decimal totalPrice, ICollection<OrderProduct> orderProducts, decimal deliveryPrice);
    }
}
