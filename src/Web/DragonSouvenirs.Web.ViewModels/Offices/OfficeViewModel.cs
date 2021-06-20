namespace DragonSouvenirs.Web.ViewModels.Offices
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class OfficeViewModel
    {
        public string OfficeBrand { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Neighborhood { get; set; }

        public string Street { get; set; }

        public string StreetNumber { get; set; }
    }
}
