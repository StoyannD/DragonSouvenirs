namespace DragonSouvenirs.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Administration.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class ProductsController : Controller
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;

        public ProductsController(
            IProductsService productsService,
            ICategoriesService categoriesService)
        {
            this.productsService = productsService;
            this.categoriesService = categoriesService;
        }

        public async Task<ActionResult> All()
        {
            var viewModel = new AllProductsViewModel
            {
                Products = await this.productsService
                .GetAllAdminAsync<AdminProductViewModel>(),
            };

            this.TempData.Keep();

            return this.View(viewModel);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var viewModel = await this.productsService
                .AdminGetByIdAsync<AdminProductViewModel>(id);

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
            var postTitle = await this.productsService.DeleteRecoverAsync(id);

            this.TempData["success"] = string.Format(GlobalConstants.Product.ProductSuccessfullyDeleted, postTitle);

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var viewModel = await this.productsService
                .AdminGetByIdAsync<AdminProductEditViewModel>(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            while (viewModel.Images.Count < GlobalConstants.Image.ImagesPerProduct)
            {
                viewModel.Images.Add(new AdminImagesViewModel());
            }

            var allCategories = await this.categoriesService
                .GetAllAsync<CategoriesDropdownViewModel>();

            viewModel.AllCategoriesDropdown = allCategories;

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<ActionResult> EditPost(AdminProductEditViewModel viewModel)
        {
        if (!this.ModelState.IsValid)
        {
            return this.View(viewModel);
        }

        await this.productsService.EditAsync(viewModel);

        this.TempData["success"] = string
            .Format(GlobalConstants.Product.ProductSuccessfullyEdited, viewModel.Name);

        return this.RedirectToAction(nameof(this.All));
        }
    }
}
