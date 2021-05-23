namespace DragonSouvenirs.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Common.Enums;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Categories;
    using DragonSouvenirs.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : Controller
    {
        private readonly IProductsService productsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ProductsController(
            IProductsService productsService,
            UserManager<ApplicationUser> userManager)
        {
            this.productsService = productsService;
            this.userManager = userManager;
        }

        public async Task<ActionResult> ByName(string title)
        {
            if (title == null)
            {
                return this.BadRequest();
            }

            var viewmodel = await this.productsService
                .GetByNameAsync<ProductViewModel>(title);

            if (viewmodel == null)
            {
                return this.NotFound();
            }

            this.TempData["Url"] = this.Request.Path.Value;
            return this.View(viewmodel);
        }

        [Route(
           "/Shop",
           Name = "shopRoute")]
        public async Task<ActionResult> Index(int? minPrice, int? maxPrice, SortBy sortBy, int page = 1, int perPage = GlobalConstants.Product.PerPageDefault)
        {
            var viewModel = new ShopViewModel
            {
                CategoryPaginationInfo = new CategoryPaginationInfo(),
            };

            if (minPrice != null && maxPrice != null)
            {
                viewModel.Products = await this.productsService
                        .GetAllAsync<ProductInCategoryViewModel>(perPage, (page - 1) * perPage, sortBy, minPrice, maxPrice);
            }
            else
            {
                viewModel.Products = await this.productsService
                    .GetAllAsync<ProductInCategoryViewModel>(perPage, (page - 1) * perPage, sortBy);
            }

            var count = await this.productsService.GetCountAsync(minPrice, maxPrice);
            viewModel.CategoryPaginationInfo.PagesCount =
                (int)Math.Ceiling((double)count / perPage);
            if (viewModel.CategoryPaginationInfo.PagesCount == 0)
            {
                viewModel.CategoryPaginationInfo.PagesCount = 1;
            }

            viewModel.CategoryPaginationInfo.MinPrice = minPrice;
            viewModel.CategoryPaginationInfo.MaxPrice = maxPrice;
            viewModel.CategoryPaginationInfo.SortBy = sortBy;
            viewModel.CategoryPaginationInfo.ProductsPerPage = perPage;
            viewModel.CategoryPaginationInfo.CurrentPage = page;
            viewModel.CategoryPaginationInfo.AllProductsCount = count;
            viewModel.CategoryPaginationInfo.Route = "shopRoute";

            this.TempData["Url"] = this.Request.Path.Value;
            return this.View(viewModel);
        }

        [Route(
            "/Favourites",
            Name = "favouritesRoute")]
        public async Task<ActionResult> Favourites()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = new FavouriteProductsViewModel
            {
                Products = await this.productsService
                .GetFavouriteProductsAsync<FavouriteProductViewModel>(user.Id),
            };

            this.TempData["Url"] = this.Request.Path.Value;
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [Route(
            "/Favourite",
            Name = "favouriteRoutePost")]
        public async Task<ActionResult> Favourite(string title)
        {
            if (title == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var success = await this.productsService
                .FavouriteProductAsync(user.Id, title);

            var url = this.TempData["Url"].ToString();
            this.TempData.Remove("Url");

            return this.Redirect(url);
        }
    }
}
