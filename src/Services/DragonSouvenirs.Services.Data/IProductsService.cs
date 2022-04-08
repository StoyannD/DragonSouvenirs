namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common.Enums;
    using DragonSouvenirs.Web.ViewModels.Administration.Products;

    public interface IProductsService
    {
        Task<IEnumerable<T>> GetAllAdminAsync<T>();

        Task<IEnumerable<T>> GetDeletedAsync<T>();

        Task<IEnumerable<T>> GetAllAsync<T>(int take, int skip, SortBy sortBy = SortBy.MostPopular, int? minPrice = null, int? maxPrice = null, string searchString = null);

        Task<IEnumerable<T>> GetAllByCategoryNameAsync<T>(string name, int take, int skip, SortBy sortBy = SortBy.MostPopular, int? minPrice = null, int? maxPrice = null, string searchString = null);

        Task<int> GetCountByCategoryIdAsync(int categoryId, int? minPrice = null, int? maxPrice = null);

        Task<IEnumerable<T>> GetTopDiscountedItems<T>(int take = 8);

        Task<IEnumerable<T>> GetFavouriteProductsAsync<T>(string userId);

        Task<bool> FavouriteProductAsync(string userId, string name);

        Task<int> GetCountAsync(int? minPrice = null, int? maxPrice = null);

        Task<T> GetByIdAsync<T>(int? id);

        Task<T> GetByNameAsync<T>(string name);

        Task<T> AdminGetByIdAsync<T>(int? id);

        Task<string> DeleteRecoverAsync(int id);

        Task<string> HardDeleteAsync(int id);

        Task EditAsync(AdminProductEditViewModel viewModel);

        Task CreateAsync(AdminProductInputModel inputModel);

        Task<decimal> MostExpensiveProductPrice();

        Task<decimal> LeastExpensiveProductPrice();
    }
}
