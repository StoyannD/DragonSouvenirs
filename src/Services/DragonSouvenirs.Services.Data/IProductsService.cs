namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DragonSouvenirs.Web.ViewModels.Administration.Products;

    public interface IProductsService
    {
        Task<IEnumerable<T>> GetAllAdminAsync<T>();

        Task<IEnumerable<T>> GetAllByCategoryNameAsync<T>(string name);

        Task<T> GetByIdAsync<T>(int? id);

        Task<T> AdminGetByIdAsync<T>(int? id);

        Task<int> DeleteRecoverAsync(int id);

        Task EditAsync(AdminProductEditViewModel viewModel);
    }
}
