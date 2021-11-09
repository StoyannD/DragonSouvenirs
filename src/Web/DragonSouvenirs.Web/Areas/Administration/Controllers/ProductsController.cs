namespace DragonSouvenirs.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Administration.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class ProductsController : BaseAdminController
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

        public async Task<ActionResult> Index()
        {
            var viewModel = new AllProductsViewModel
            {
                Products = await this.productsService
                .GetAllAdminAsync<AdminProductViewModel>(),
            };

            this.TempData.Keep();

            return this.View(viewModel);
        }

        public async Task<ActionResult> Deleted()
        {
            var viewModel = new AllProductsViewModel
            {
                Products = await this.productsService
                    .GetDeletedAsync<AdminProductViewModel>(),
            };

            this.TempData.Keep();

            return View("Index", viewModel);
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

            return RedirectToAction("Index", "Products", new { area = "Administration" });
        }

        // [HttpPost]
        // public async Task<ActionResult> HardDelete(int id)
        // {
        //    var postTitle = await this.productsService.HardDeleteAsync(id);

        // this.TempData["success"] = string.Format(GlobalConstants.Product.ProductSuccessfullyDeleted, postTitle);

        // return RedirectToAction("Index", "Products", new { area = "Administration" });
        // }
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

            var categories = await this.categoriesService.GetAllByProductIdAsync<CategoriesViewModel>(id);
            viewModel.Categories = categories.ToList();

            // Find a way to do it smarter
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

            foreach (var image in viewModel.Images)
            {
                if (image.Image != null)
                {
                    var fileType = image.Image.ContentType.Split('/')[1];
                    if (!this.IsImageFileValidType(fileType))
                    {
                        return this.View(viewModel);
                    }
                }
            }

            await this.productsService.EditAsync(viewModel);

            this.TempData["success"] = string
                .Format(GlobalConstants.Product.ProductSuccessfullyEdited, viewModel.Name);

            return RedirectToAction("Index", "Products", new { area = "Administration" });
        }

        public async Task<ActionResult> Create()
        {
            var categoriesDropdown = await this.categoriesService
                .GetAllAsync<CategoriesDropdownViewModel>();

            var inputModel = new AdminProductInputModel
            {
                AllCategoriesDropdown = categoriesDropdown,
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(AdminProductInputModel inputModel)
        {
            if (inputModel == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            foreach (var image in inputModel.Images)
            {
                if (image != null)
                {
                    var fileType = image.Image.ContentType.Split('/')[1];
                    if (!this.IsImageFileValidType(fileType))
                    {
                        return this.View(inputModel);
                    }
                }
            }

            await this.productsService
                    .CreateAsync(inputModel);

            this.TempData["success"] =
                string.Format(GlobalConstants.Product.ProductSuccessfullyCreated, inputModel.Title);

            return RedirectToAction("Index", "Products", new { area = "Administration" });
        }
    }
}
