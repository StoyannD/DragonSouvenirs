namespace DragonSouvenirs.Web.Areas.Administration.Controllers
{
    using DragonSouvenirs.Common;
    using DragonSouvenirs.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
