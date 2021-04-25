namespace DragonSouvenirs.Data.Models
{
    using DragonSouvenirs.Data.Common.Models;

    public class Image : BaseDeletableModel<int>
    {
        public string ImgUrl { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
