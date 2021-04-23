namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductsService
    {
        IEnumerable<T> GetByCategoryName<T>(string name);

        T GetById<T>(int id);
    }
}
