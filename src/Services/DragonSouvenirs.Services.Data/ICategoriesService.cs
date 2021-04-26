namespace DragonSouvenirs.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICategoriesService
    {
        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAllAdmin<T>();

        Task<T> GetByNameAsync<T>(string name);

        Task<T> GetByIdAsync<T>(int id);

        Task<string> DeleteRecoverAsync(int id);
    }
}
