namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICartService
    {
        Task<IEnumerable<T>> GetCartProductsAsync<T>(string userId);

        Task AddProductToCartAsync(string userId, int productId);

        Task DeleteProductFromCartAsync(string userId, int productId);

        //Task<T> GetCartProductByIdAsync<T>(int productId);

        Task EditProductInCartAsync(string userId, int productId, int quantity);
    }
}
