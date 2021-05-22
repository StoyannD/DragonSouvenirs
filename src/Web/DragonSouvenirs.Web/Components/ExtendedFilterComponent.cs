namespace DragonSouvenirs.Web.Components
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper.Internal;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Components.ExtendedFilterComponent;
    using Microsoft.AspNetCore.Mvc;

    public class ExtendedFilterComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;
        private readonly IProductsService productsService;

        public ExtendedFilterComponent(
            ICategoriesService categoriesService,
            IProductsService productsService)
        {
            this.categoriesService = categoriesService;
            this.productsService = productsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string currentCategory)
        {
            var viewModel = new ExtendedFilterViewModel
            {
                Categories = await this.categoriesService.GetAllAsync<CategoryViewModel>(),
                CurrentCategory = currentCategory,
                MinPrice = (int)await
                    this.productsService.LeastExpensiveProductPrice(),
                MaxPrice = (int)Math.Ceiling(await this.productsService.MostExpensiveProductPrice()),
        };

            viewModel.Categories.ForAll(c => c.IsSelected = false);
            return this.View(viewModel);
        }
    }
}
