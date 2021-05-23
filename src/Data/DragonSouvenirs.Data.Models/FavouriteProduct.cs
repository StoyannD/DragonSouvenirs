namespace DragonSouvenirs.Data.Models
{
    using System;

    using DragonSouvenirs.Data.Common.Models;

    public class FavouriteProduct : IDeletableEntity
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
