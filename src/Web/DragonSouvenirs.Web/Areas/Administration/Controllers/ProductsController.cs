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

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
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
    }
}
