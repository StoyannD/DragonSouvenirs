namespace DragonSouvenirs.Web.Areas.Administration.Controllers
{
    using System;
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

            viewModel.Users = users;
            this.TempData.Keep();

            return this.View(viewModel);
        }

        public async Task<ActionResult> Ban(string id)
        {
            var viewModel = await this.userManager
                .Users
                .IgnoreQueryFilters()
                .To<UserConfirmModel>()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            if (viewModel.IsDeleted)
            {
                this.TempData["fail"] =
                    string.Format(GlobalConstants.UserAlreadyBannedMessage, viewModel.UserName);
                return this.RedirectToAction(nameof(this.All));
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Ban")]
        public async Task<ActionResult> BanPost(string id)
        {
            var user = await this.userManager
                .FindByIdAsync(id);

            if (user == null)
            {
                return this.NotFound();
            }

            user.IsDeleted = true;
            user.DeletedOn = DateTime.UtcNow;

            await this.userManager.UpdateAsync(user);

            this.TempData["success"] =
                string.Format(GlobalConstants.UserSuccessfullyBannedMessage, user.UserName);

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        public async Task<ActionResult> UnBan(string id)
        {
            var user = await this.userManager
                .Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (!user.IsDeleted)
            {
                this.TempData["fail"] =
                    string.Format(GlobalConstants.UserNotBannedMessage, user.UserName);
                return this.RedirectToAction(nameof(this.All));
            }

            user.IsDeleted = false;
            user.DeletedOn = null;

            await this.userManager.UpdateAsync(user);

            this.TempData["success"] =
                string.Format(GlobalConstants.UserSuccessfullyUnBannedMessage, user.UserName);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
