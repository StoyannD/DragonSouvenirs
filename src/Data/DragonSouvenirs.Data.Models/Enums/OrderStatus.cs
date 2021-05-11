namespace DragonSouvenirs.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum OrderStatus
    {
        Created = 0,

        [Display(Name = "В процес на обработка")]
        Processing = 1,

        [Display(Name = "Обработена")]
        Processed = 2,

        [Display(Name = "Доставена")]
        Delivered = 3,
    }
}
