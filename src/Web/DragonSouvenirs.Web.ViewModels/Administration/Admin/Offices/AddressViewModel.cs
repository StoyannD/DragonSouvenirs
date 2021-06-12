namespace DragonSouvenirs.Web.ViewModels.Administration.Admin.Offices
{
    public class AddressViewModel
    {
        public string FullAddress { get; set; }

        public string Quarter { get; set; }

        public string Street { get; set; }

        public string Num { get; set; }

        public CityViewModel City { get; set; }
    }
}
