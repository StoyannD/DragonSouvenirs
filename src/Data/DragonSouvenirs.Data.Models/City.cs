namespace DragonSouvenirs.Data.Models
{
    using DragonSouvenirs.Data.Common.Models;

    public class City : BaseDeletableModel<int>
    {
        public string Name { get; set; }
    }
}
