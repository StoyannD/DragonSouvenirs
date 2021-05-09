namespace DragonSouvenirs.Web.Controllers
{
    using System.Threading.Tasks;

    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : Controller
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<ActionResult> ById(int? id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var viewmodel = await this.productsService
                .GetByIdAsync<ProductViewModel>(id);

            if (viewmodel == null)
            {
                return this.NotFound();
            }

            return this.View(viewmodel);
        }
    }
}
