namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DragonSouvenirs.Web.ViewModels.Administration.Categories;

    public interface ICategoriesService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllAdminAsync<T>();

        Task<T> GetByNameAsync<T>(string name);

        Task<T> GetByIdAsync<T>(int id);

        Task<string> DeleteRecoverAsync(int id);

        Task EditAsync(AdminCategoryEditViewModel viewModel);

        Task CreateAsync(AdminCategoryInputModel inputModel);
    }
}
