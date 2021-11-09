namespace DragonSouvenirs.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels;
    using DragonSouvenirs.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly ICategoriesService categoriesService;

        public HomeController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public async Task<ActionResult> Index()
        {
            var viewModel = new IndexViewModel();

            var categories = await this.categoriesService.GetAllAsync<IndexCategoryViewModel>();
            if (categories == null)
            {
                viewModel.Categories = new List<IndexCategoryViewModel>();
            }

            viewModel.Categories = categories;

            this.TempData.Keep();

            return this.View(viewModel);
        }

        [Route(
            "/Contact",
            Name = "contactRoute")]
        public IActionResult Contact()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult ForUs()
        {
            return this.View();
        }

        public IActionResult DeliveryInformation()
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
