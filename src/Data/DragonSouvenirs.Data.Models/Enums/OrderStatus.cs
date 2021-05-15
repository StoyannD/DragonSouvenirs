namespace DragonSouvenirs.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum OrderStatus
    {
        [Display(Name = "Създадена")]
        Created = 0,

        [Display(Name = "В процес на обработка")]
        Processing = 1,

        [Display(Name = "Обработена")]
        Processed = 2,

        [Display(Name = "Изпратена")]
        Sent = 3,

        [Display(Name = "Доставена")]
        Delivered = 4,

        [Display(Name = "Завършена")]
        Completed = 5,
    }
}
