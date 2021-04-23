namespace DragonSouvenirs.Web.Controllers
{
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Categories;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IProductsService productsService;

        public CategoriesController(
            ICategoriesService categoriesService,
            IProductsService productsService)
        {
            this.categoriesService = categoriesService;
            this.productsService = productsService;
        }

        public IActionResult ByName(string name)
        {
            var viewModel = this.categoriesService
                .GetByName<CategoryViewModel>(name);

            viewModel.Products = this.productsService.GetByCategoryName<ProductInCategoryViewModel>(name);

            return this.View(viewModel);
        }
    }
}
