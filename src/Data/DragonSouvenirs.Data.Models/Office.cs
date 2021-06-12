namespace DragonSouvenirs.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DragonSouvenirs.Data.Common.Models;

    public class Office : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Neighborhood { get; set; }

        public string Street { get; set; }

        public string StreetNumber { get; set; }
    }
}
