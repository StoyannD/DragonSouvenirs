namespace DragonSouvenirs.Services.Messaging
{
    using System.Collections.Generic;

    using DragonSouvenirs.Data.Models;

    public interface IEmailTemplatesSender
    {
        public string Order(string orderTitle, string shippingAddress, string clientFullName, string invoiceNumber,
            decimal totalPrice, ICollection<OrderProduct> orderProducts, decimal deliveryPrice);
    }
}
