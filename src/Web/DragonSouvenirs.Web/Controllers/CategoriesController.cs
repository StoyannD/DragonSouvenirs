namespace DragonSouvenirs.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Common.Enums;
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

        public async Task<ActionResult> ByName(int? minPrice, int? maxPrice, SortBy sortBy, string name,
            int page = 1, int perPage = GlobalConstants.Product.PerPageDefault)
        {
            if (name == null)
            {
                return this.BadRequest();
            }

            var viewModel = await this.categoriesService
                .GetByNameAsync<CategoryViewModel>(name);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            viewModel.CategoryPaginationInfo = new CategoryPaginationInfo();

            if (minPrice != null && maxPrice != null)
            {
                viewModel.Products = await this.productsService
                    .GetAllByCategoryNameAsync<ProductInCategoryViewModel>(name, perPage, (page - 1) * perPage, sortBy, minPrice, maxPrice);
            }
            else
            {
                viewModel.Products = await this.productsService
                    .GetAllByCategoryNameAsync<ProductInCategoryViewModel>(name, perPage, (page - 1) * perPage, sortBy);
            }

            var count = await this.productsService.GetCountByCategoryIdAsync(viewModel.Id, minPrice, maxPrice);
            viewModel.CategoryPaginationInfo.PagesCount = (int)Math.Ceiling((double)count / perPage);
            if (viewModel.CategoryPaginationInfo.PagesCount == 0)
            {
                viewModel.CategoryPaginationInfo.PagesCount = 1;
            }

            viewModel.CategoryPaginationInfo.MinPrice = minPrice;
            viewModel.CategoryPaginationInfo.MaxPrice = maxPrice;
            viewModel.CategoryPaginationInfo.SortBy = sortBy;
            viewModel.CategoryPaginationInfo.CurrentPage = page;
            viewModel.CategoryPaginationInfo.ProductsPerPage = perPage;
            viewModel.CategoryPaginationInfo.AllProductsCount = count;
            viewModel.CategoryPaginationInfo.CategoryName = name;
            viewModel.CategoryPaginationInfo.Route = "categoriesByName";

            this.TempData["Url"] = this.Request.Path.Value;
            return this.View(viewModel);
        }
    }
}
