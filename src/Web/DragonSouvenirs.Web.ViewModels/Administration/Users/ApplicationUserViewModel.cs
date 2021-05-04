namespace DragonSouvenirs.Web.ViewModels.Administration.Users
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class ApplicationUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string DefaultShippingAddress { get; set; }

        public string ShortShippingAddress
        {
            get
            {
                if (this.DefaultShippingAddress == null)
                {
                    return string.Empty;
                }

                var content = this.DefaultShippingAddress.Length > 30
                    ? this.DefaultShippingAddress.Substring(0, 30) + "..."
                    : this.DefaultShippingAddress;
                return content;
            }
        }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
