namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductsService
    {
        Task<IEnumerable<T>> GetByCategoryNameAsync<T>(string name);

        Task<T> GetByIdAsync<T>(int id);
    }
}
