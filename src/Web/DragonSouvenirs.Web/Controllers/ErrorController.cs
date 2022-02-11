namespace DragonSouvenirs.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : Controller
    {
        public IActionResult PageNotFound()
        {
            return this.View();
        }
    }
}
