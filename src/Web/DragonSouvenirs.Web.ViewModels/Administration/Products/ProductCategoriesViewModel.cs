namespace DragonSouvenirs.Web.ViewModels.Administration.Products
{
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;

    public class ProductCategoriesViewModel : IMapFrom<ProductCategory>
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
