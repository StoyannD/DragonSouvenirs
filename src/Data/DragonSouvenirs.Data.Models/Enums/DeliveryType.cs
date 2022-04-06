namespace DragonSouvenirs.Data.Models.Enums
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum DeliveryType
    {
        [Display(Name = "До Офис")]
        ToOffice = 0,
        [Display(Name = "До Адрес")]
        ToAddress = 1,
    }
}
