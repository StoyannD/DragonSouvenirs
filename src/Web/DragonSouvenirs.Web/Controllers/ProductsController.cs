namespace DragonSouvenirs.Web.Controllers
{
    using System;
    using System.Linq;
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

        public async Task<ActionResult> ByName(string name)
        {
            if (name == null)
            {
                return this.BadRequest();
            }

            var viewmodel = await this.productsService
                .GetByNameAsync<ProductViewModel>(name);

            if (viewmodel == null)
            {
                return this.NotFound();
            }

            this.TempData["Url"] = this.Request.Path.Value;
            return this.View(viewmodel);
        }

        [Route(
           "/Shop",
           Name = GlobalConstants.Routes.ShopRoute)]
        public async Task<ActionResult> Index(string searchString, int? minPrice, int? maxPrice, SortBy sortBy,
            int page = 1, int perPage = GlobalConstants.Product.PerPageDefault)
        {
            var viewModel = new ShopViewModel
            {
                CategoryPaginationInfo = new CategoryPaginationInfo(),
                Products = await this.productsService
                    .GetAllAsync<ProductInCategoryViewModel>(perPage, (page - 1) * perPage, sortBy, minPrice, maxPrice),
            };

            // Search products that contain all the words in the search string in their name
            // Ef cannot translate query to sql
            if (searchString != null)
            {
                var searchStringArr = searchString
                    .Split(new[] { ",", ".", " ", "\\", "/", "|", "!", "?" }, StringSplitOptions.RemoveEmptyEntries);

                viewModel.Products = viewModel.Products.Where(p =>
                    searchStringArr.All(ss => p.Title.ToLower().Contains(ss.ToLower())));
            }

            // Calculate the count of the filtered product
            var count = searchString == null
                ? await this.productsService.GetCountAsync(minPrice, maxPrice)
                : viewModel.Products.Count();

            viewModel.CategoryPaginationInfo.PagesCount =
                (int)Math.Ceiling((double)count / perPage);
            if (viewModel.CategoryPaginationInfo.PagesCount == 0)
            {
                viewModel.CategoryPaginationInfo.PagesCount = 1;
            }

            viewModel.CategoryPaginationInfo.SearchString = searchString;

            viewModel.CategoryPaginationInfo.MinPrice = minPrice;
            viewModel.CategoryPaginationInfo.MaxPrice = maxPrice;
            viewModel.CategoryPaginationInfo.SortBy = sortBy;
            viewModel.CategoryPaginationInfo.ProductsPerPage = perPage;
            viewModel.CategoryPaginationInfo.CurrentPage = page;
            viewModel.CategoryPaginationInfo.AllProductsCount = count;
            viewModel.CategoryPaginationInfo.Route = GlobalConstants.Routes.ShopRoute;

            this.TempData["Url"] = this.Request.Path.Value;
            return this.View(viewModel);
        }

        [Authorize]
        [Route(
            "/Favourites",
            Name = GlobalConstants.Routes.FavouritesRoute)]
        public async Task<ActionResult> Favourites()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = new FavouriteProductsViewModel();

            if (user != null)
            {
                viewModel.Products = await this.productsService
                    .GetFavouriteProductsAsync<FavouriteProductViewModel>(user.Id);
            }

            this.TempData["Url"] = this.Request.Path.Value;
            return this.View(viewModel);
        }

        [Authorize]
        public async Task<ActionResult> Favourite(string name)
        {
            if (name == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var success = await this.productsService
                .FavouriteProductAsync(user.Id, name);

            if (!success)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var url = this.TempData["Url"].ToString();
            this.TempData.Remove("Url");

            if (url!.StartsWith("/Products"))
            {
                url += Request.QueryString;
            }

            return this.Redirect(url);
        }
    }
}
