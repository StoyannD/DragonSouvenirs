namespace DragonSouvenirs.Web.Components
{
    using System.Threading.Tasks;

    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Components.DiscountedProductsComponent;
    using Microsoft.AspNetCore.Mvc;

    public class DiscountedProductsComponent : ViewComponent
    {
        private readonly IProductsService productsService;

        public DiscountedProductsComponent(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await this.productsService
                .GetTopDiscountedItems<DiscountedProductsViewModel>();

            return this.View(products);
        }
    }
}
