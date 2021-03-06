// ReSharper disable VirtualMemberCallInConstructor

namespace DragonSouvenirs.Data.Models
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using DragonSouvenirs.Data.Common.Models;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Orders = new HashSet<Order>();
            this.FavouriteProducts = new HashSet<FavouriteProduct>();
        }

        public string FullName { get; set; }

        public string DefaultShippingAddress { get; set; }

        public string City { get; set; }

        public string Neighborhood { get; set; }

        public string Street { get; set; }

        public int StreetNumber { get; set; }

        public string ApartmentBuilding { get; set; }

        public string Entrance { get; set; }

        public int Floor { get; set; }

        public int ApartmentNumber { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }

        public int PersonalDiscountPercentage { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<FavouriteProduct> FavouriteProducts { get; set; }
    }
}
