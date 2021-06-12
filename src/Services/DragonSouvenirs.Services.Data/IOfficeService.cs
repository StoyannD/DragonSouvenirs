namespace DragonSouvenirs.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DragonSouvenirs.Web.ViewModels.Offices;

    public interface IOfficeService
    {
        Task UpdateOfficesAsync();

        Task<IEnumerable<OfficeViewModel>> GetAllOfficesAsync();

        Task<IEnumerable<CityViewModel>> GetAllCitiesAsync();
    }
}
