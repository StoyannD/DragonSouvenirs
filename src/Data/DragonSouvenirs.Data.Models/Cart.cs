namespace DragonSouvenirs.Data.Models
{
    using System.Collections.Generic;

    using DragonSouvenirs.Data.Common.Models;

    public class Cart : BaseDeletableModel<int>
    {
        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual IEnumerable<CartProduct> CartProducts { get; set; }
    }
}
