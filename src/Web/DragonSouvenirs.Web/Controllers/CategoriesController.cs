namespace DragonSouvenirs.Web.Controllers
{
    using System.Threading.Tasks;

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

        public async Task<ActionResult> ByName(string name)
        {
            var viewModel = await this.categoriesService
                .GetByNameAsync<CategoryViewModel>(name);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            viewModel.Products = await this.productsService
                .GetAllByCategoryNameAsync<ProductInCategoryViewModel>(name);

            return this.View(viewModel);
        }
    }
}
