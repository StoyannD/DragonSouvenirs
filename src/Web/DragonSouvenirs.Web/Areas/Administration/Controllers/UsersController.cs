using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace DragonSouvenirs.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels.Administration;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ActionResult> All()
        {
            var viewModel = new AllUsersViewModel();
            var users = await this.userManager
                .Users
                .IgnoreQueryFilters()
                .OrderBy(u => u.CreatedOn)
                .To<ApplicationUserViewModel>()
                .ToListAsync();

            viewModel.users = users;
            this.TempData.Keep();

            return this.View(viewModel);
        }

        public async Task<ActionResult> ConfirmBan(string id)
        {
            var viewModel = await this.userManager
                .Users
                .IgnoreQueryFilters()
                .To<UserConfirmModel>()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (viewModel.IsDeleted)
            {
                this.TempData["isBanned"] = $"User {viewModel.UserName} is already banned.";
                return this.RedirectToAction(nameof(this.All));
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Ban(string id)
        {
            var user = await this.userManager
                .FindByIdAsync(id);

            user.IsDeleted = true;

            await this.userManager.UpdateAsync(user);

            return this.Redirect("/Administration/Users/All");
        }
    }
}
