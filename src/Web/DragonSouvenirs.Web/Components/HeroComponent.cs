namespace DragonSouvenirs.Web.Components
{
    using System.Threading.Tasks;

    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Components.HeroComponent;
    using Microsoft.AspNetCore.Mvc;

    public class HeroComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;

        public HeroComponent(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryName = null, bool isIndex = false, int? minPrice = null, int? maxPrice = null)
        {
            var model = new CategoriesViewModel
            {
                IsIndex = isIndex,
                CategoryName = categoryName,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Categories = await this.categoriesService.GetAllAsync<CategoryViewModel>(),
            };
            return View(model);
        }
    }
}
