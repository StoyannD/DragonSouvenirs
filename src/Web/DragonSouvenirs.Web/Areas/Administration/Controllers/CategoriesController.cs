namespace DragonSouvenirs.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Administration.Categories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Server.IIS.Core;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult All()
        {
            var viewModel = new AllCategoriesViewModel();

            var categories = this.categoriesService
                .GetAllAdmin<AdminCategoryViewModel>();
            viewModel.Categories = categories;

            this.TempData.Keep();

            return this.View(viewModel);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var viewModel = await this.categoriesService
                .GetByIdAsync<AdminCategoryViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        // [HttpPost]
        // [ActionName("Delete")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var title = await this.categoriesService
                .DeleteRecoverAsync(id);

            if (title == null)
            {
                return this.NotFound();
            }

            this.TempData["success"] = string.Format(GlobalConstants.CategorySuccessfullyDeleted, title);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
