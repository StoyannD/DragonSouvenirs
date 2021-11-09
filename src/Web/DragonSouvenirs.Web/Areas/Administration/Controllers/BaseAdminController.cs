namespace DragonSouvenirs.Web.Areas.Administration.Controllers
{
    using DragonSouvenirs.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class BaseAdminController : Controller
    {
        protected bool IsImageFileValidType(string fileType)
        {
            return fileType is "jpeg" or "jpg" or "png";
        }
    }
}
