namespace DragonSouvenirs.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Services.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdminController : Controller
    {
        private readonly IOfficeService officeService;

        public AdminController(IOfficeService officeService)
        {
            this.officeService = officeService;
        }

        public async Task<ActionResult> UpdateOffices()
        {
            await this.officeService.UpdateOfficesAsync();

            return RedirectToAction("Index", "Orders");
        }
    }
}
