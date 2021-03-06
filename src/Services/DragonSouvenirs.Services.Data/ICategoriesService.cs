namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DragonSouvenirs.Web.ViewModels.Administration.Categories;

    public interface ICategoriesService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetDeletedAsync<T>();

        Task<IEnumerable<T>> GetAllAdminAsync<T>();

        Task<IEnumerable<T>> GetAllByProductIdAsync<T>(int? id);

        Task<T> GetByNameAsync<T>(string name);

        Task<T> GetByIdAsync<T>(int? id);

        Task<string> DeleteRecoverAsync(int id);

        Task<string> RecoverSimpleAsync(int id);

        Task EditAsync(AdminCategoryEditViewModel viewModel);

        Task CreateAsync(AdminCategoryInputModel inputModel);
    }
}
