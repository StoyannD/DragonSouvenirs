namespace DragonSouvenirs.Web.ViewModels.Offices
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CityViewModel : IMapFrom<City>
    {
        public string Name { get; set; }
    }
}
