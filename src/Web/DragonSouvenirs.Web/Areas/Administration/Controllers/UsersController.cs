namespace DragonSouvenirs.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class UsersController : BaseAdminController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICommonFeaturesService commonFeaturesService;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            ICommonFeaturesService commonFeaturesService)
        {
            this.userManager = userManager;
            this.commonFeaturesService = commonFeaturesService;
        }

        public async Task<ActionResult> Index()
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
            if (id == null)
            {
                return this.BadRequest();
            }

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
                    string.Format(GlobalConstants.User.UserAlreadyBannedMessage, viewModel.UserName);
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Ban")]
        public async Task<ActionResult> BanPost(string id)
        {
            var user = await this.userManager
                .FindByIdAsync(id);

            user.IsDeleted = true;
            user.DeletedOn = DateTime.UtcNow;

            await this.userManager.UpdateAsync(user);

            this.TempData["success"] =
                string.Format(GlobalConstants.User.UserSuccessfullyBannedMessage, user.UserName);

            return this.RedirectToAction(nameof(this.Index));
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
                    string.Format(GlobalConstants.User.UserNotBannedMessage, user.UserName);
                return this.RedirectToAction(nameof(this.Index));
            }

            user.IsDeleted = false;

            await this.userManager.UpdateAsync(user);

            this.TempData["success"] =
                string.Format(GlobalConstants.User.UserSuccessfullyUnBannedMessage, user.UserName);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return this.BadRequest();
            }

            var viewModel = await this.userManager
                .Users
                .Where(u => u.Id == id)
                .To<ApplicationUserEditViewModel>()
                .FirstOrDefaultAsync();

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ApplicationUserEditViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            var user = await this.userManager
                .FindByIdAsync(viewModel.Id);

            user.UserName = viewModel.UserName;
            user.FullName = viewModel.FullName;
            user.Email = viewModel.Email;
            user.City = viewModel.City;
            user.Neighborhood = viewModel.Neighborhood;
            user.Street = viewModel.Street;
            user.StreetNumber = viewModel.StreetNumber;
            user.ApartmentBuilding = viewModel.ApartmentBuilding;
            user.Entrance = viewModel.Entrance;
            user.Floor = viewModel.Floor;
            user.ApartmentNumber = viewModel.ApartmentNumber;
            user.PersonalDiscountPercentage = viewModel.PersonalDiscountPercentage;

            user.DefaultShippingAddress = this.commonFeaturesService
                .RenderAddress(
                    viewModel.City,
                    viewModel.Neighborhood,
                    viewModel.Street,
                    viewModel.StreetNumber,
                    viewModel.ApartmentBuilding,
                    viewModel.Entrance,
                    viewModel.Floor,
                    viewModel.ApartmentNumber);

            await this.userManager.UpdateAsync(user);

            this.TempData["success"] =
                string.Format(GlobalConstants.User.UserSuccessfullyEdited, user.UserName);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
