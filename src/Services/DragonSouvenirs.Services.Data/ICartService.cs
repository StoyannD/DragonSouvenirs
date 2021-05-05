using DragonSouvenirs.Data.Models;

namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DragonSouvenirs.Web.ViewModels.Cart;

    public interface ICartService
    {
        Task<IEnumerable<T>> GetCartProductsAsync<T>(string userId);

        Task AddProductToCartAsync(string userId, int productId);
    }
}
