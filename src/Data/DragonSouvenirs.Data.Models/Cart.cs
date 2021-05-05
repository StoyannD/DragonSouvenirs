namespace DragonSouvenirs.Data.Models
{
    using System.Collections.Generic;

    using DragonSouvenirs.Data.Common.Models;

    public class Cart : BaseDeletableModel<int>
    {
        public Cart()
        {
            this.CartProducts = new HashSet<CartProduct>();
        }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual IEnumerable<CartProduct> CartProducts { get; set; }
    }
}
