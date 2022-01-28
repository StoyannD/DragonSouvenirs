namespace DragonSouvenirs.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum OrderStatus
    {
        [Display(Name = "Създадена")]
        Created = 0,

        [Display(Name = "В процес на обработка")]
        Processing = 1,

        [Display(Name = "Приета")]
        Accepted = 2,

        [Display(Name = "Обработена")]
        Processed = 3,

        [Display(Name = "Изпратена")]
        Sent = 4,

        [Display(Name = "Доставена")]
        Delivered = 5,

        [Display(Name = "Завършена")]
        Completed = 6,
    }
}
