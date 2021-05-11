namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICartService
    {
        Task<IEnumerable<T>> GetCartProductsAsync<T>(string userId);

        Task<decimal> GetCartTotalPriceAsync(string userId);

        Task AddProductToCartAsync(string userId, int productId);

        Task DeleteProductFromCartAsync(string userId, int productId);

        Task EditProductInCartAsync(string userId, int productId, int quantity);

        Task<T> GetCartByUsernameAsync<T>(string username);

        Task<bool> UserHasProductsInCart(string userId);
    }
}
