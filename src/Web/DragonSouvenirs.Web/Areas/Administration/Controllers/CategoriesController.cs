namespace DragonSouvenirs.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Administration.Categories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class CategoriesController : BaseAdminController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public async Task<ActionResult> Index()
        {
            var viewModel = new AllCategoriesViewModel();

            var categories = await this.categoriesService
                .GetAllAsync<AdminCategoryViewModel>();
            viewModel.Categories = categories;

            this.TempData.Keep();

            return this.View(viewModel);
        }

        public async Task<ActionResult> Deleted()
        {
            var viewModel = new AllCategoriesViewModel();

            var categories = await this.categoriesService
                .GetDeletedAsync<AdminCategoryViewModel>();
            viewModel.Categories = categories;

            this.TempData.Keep();

            return View("Index", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var viewModel = await this.categoriesService
                .GetByIdAsync<AdminCategoryViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var title = await this.categoriesService
                .DeleteRecoverAsync(id);

            this.TempData["success"] = string.Format(GlobalConstants.Category.CategorySuccessfullyDeleted, title);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var viewModel = await this.categoriesService
                .GetByIdAsync<AdminCategoryEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AdminCategoryEditViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            var fileType = viewModel.Image.ContentType.Split('/')[1];
            if (!this.IsImageFileValidType(fileType))
            {
                return this.View(viewModel);
            }

            await this.categoriesService.EditAsync(viewModel);

            this.TempData["success"] = string
                .Format(GlobalConstants.Category.CategorySuccessfullyEdited, viewModel.Title);

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(AdminCategoryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                var fileType = inputModel.Image.ContentType.Split('/')[1];
                if (!this.IsImageFileValidType(fileType))
                {
                    return this.View(inputModel);
                }

                await this.categoriesService
                    .CreateAsync(inputModel);

                this.TempData["success"] =
                    string.Format(GlobalConstants.Category.CategorySuccessfullyCreated, inputModel.Title);
            }
            catch (Exception e)
            {
                this.TempData["fail"] = e.Message;
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
