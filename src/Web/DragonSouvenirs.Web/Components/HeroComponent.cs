namespace DragonSouvenirs.Web.Components
{
    using System.Threading.Tasks;

    using DragonSouvenirs.Common.Enums;
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

        public async Task<IViewComponentResult> InvokeAsync(
            string categoryName, SortBy sortBy, int? currentPage, int? productsPerPage,
            bool isIndex = false, int? minPrice = null, int? maxPrice = null)
        {
            var model = new CategoriesViewModel
            {
                CategoryName = categoryName,
                SortBy = sortBy,
                CurrentPage = currentPage,
                ProductsPerPage = productsPerPage,
                IsIndex = isIndex,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Categories = await this.categoriesService.GetAllAsync<CategoryViewModel>(),
            };
            return View(model);
        }
    }
}
