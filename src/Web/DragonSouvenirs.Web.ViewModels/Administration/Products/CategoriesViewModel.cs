namespace DragonSouvenirs.Web.ViewModels.Administration.Products
{
    using System.ComponentModel.DataAnnotations;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class CategoriesViewModel : IMapFrom<Category>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
