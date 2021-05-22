namespace DragonSouvenirs.Web.Components
{
    using System.Threading.Tasks;

    using DragonSouvenirs.Common.Enums;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Components.LatestProductsComponent;
    using Microsoft.AspNetCore.Mvc;

    public class LatestProductsComponent : ViewComponent
    {
        private readonly IProductsService productsService;

        public LatestProductsComponent(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await this.productsService
                .GetAllAsync<LatestProductsViewModel>(6, 0, SortBy.Newest);

            return this.View(products);
        }
    }
}
