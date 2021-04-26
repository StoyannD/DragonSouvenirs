namespace DragonSouvenirs.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels;
    using DragonSouvenirs.Web.ViewModels.Categories;
    using DragonSouvenirs.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly ICategoriesService categoriesService;

        public HomeController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel();

            var categories = this.categoriesService.GetAll<IndexCategoryViewModel>();
            if (categories == null)
            {
                viewModel.Categories = new List<IndexCategoryViewModel>();
            }

            viewModel.Categories = categories;

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            this.TempData.Keep();
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
