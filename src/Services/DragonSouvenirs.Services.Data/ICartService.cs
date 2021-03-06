namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICartService
    {
        Task<IEnumerable<T>> GetCartProductsAsync<T>(string userId);

        Task<decimal> GetCartTotalPriceAsync(string userId, decimal personalDiscountPercentage);

        Task<bool> AddProductToCartAsync(string userId, int productId, int quantity);

        Task<bool> DeleteProductFromCartAsync(string userId, int productId);

        Task<bool> EditProductInCartAsync(string userId, int productId, int quantity);

        Task<T> GetCartByIdAsync<T>(string id);

        Task<bool> UserHasProductsInCart(string userId);
    }
}
