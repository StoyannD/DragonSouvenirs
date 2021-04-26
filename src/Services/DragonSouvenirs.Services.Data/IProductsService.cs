namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductsService
    {
        Task<IEnumerable<T>> GetAllAdminAsync<T>();

        Task<IEnumerable<T>> GetAllByCategoryNameAsync<T>(string name);

        Task<T> GetByIdAsync<T>(int id);
    }
}
