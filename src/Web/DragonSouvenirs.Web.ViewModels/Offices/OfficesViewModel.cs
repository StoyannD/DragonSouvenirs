namespace DragonSouvenirs.Web.ViewModels.Offices
{
    using System.Collections.Generic;

    public class OfficesViewModel
    {
        public IEnumerable<OfficeViewModel> Offices { get; set; }

        public IEnumerable<OfficeViewModel> EcontOffices { get; set; }

        public IEnumerable<OfficeViewModel> SpeedyOffices { get; set; }
    }
}
