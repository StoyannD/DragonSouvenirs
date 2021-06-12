namespace DragonSouvenirs.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Models.Enums;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Offices;
    using DragonSouvenirs.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderService orderService;
        private readonly ICartService cartService;
        private readonly IProductsService productsService;
        private readonly IOfficeService officeService;

        public OrdersController(
            UserManager<ApplicationUser> userManager,
            IOrderService orderService,
            ICartService cartService,
            IProductsService productsService,
            IOfficeService officeService)
        {
            this.userManager = userManager;
            this.orderService = orderService;
            this.cartService = cartService;
            this.productsService = productsService;
            this.officeService = officeService;
        }

        public async Task<ActionResult> Create()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!await this.cartService.UserHasProductsInCart(user.Id))
            {
                this.TempData["fail"] = GlobalConstants.Order.EmptyCart;
                return this.RedirectToAction("Index", "Home");
            }

            var offices = new OfficesViewModel
            {
                Offices = await this.officeService.GetAllOfficesAsync(),
            };

            var cities = new CitiesViewModel
            {
                Cities = await this.officeService.GetAllCitiesAsync(),
            };

            var viewModel = new CreateOrderViewModel
            {
                UserFullName = user.FullName,
                FirstName = user.FullName.Split(' ').First(),
                LastName = user.FullName[user.FullName.IndexOf(" ", StringComparison.Ordinal)..],
                UserEmail = user.Email,
                InvoiceNumber = user.PhoneNumber,
                UserCity = user.City,
                UserNeighborhood = user.Neighborhood,
                UserStreet = user.Street,
                UserStreetNumber = user.StreetNumber,
                UserApartmentBuilding = user.ApartmentBuilding,
                UserEntrance = user.Entrance,
                UserFloor = user.Floor,
                UserApartmentNumber = user.ApartmentNumber,
                Offices = offices,
                Cities = cities,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<ActionResult> CreatePost(CreateOrderViewModel inputModel)
        {
            if (inputModel == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            if (inputModel.DeliveryType == DeliveryType.ToOffice)
            {
                if (inputModel.OfficeName == null)
                {
                    return this.RedirectToAction(nameof(this.Create));
                }
            }

            if (inputModel.DeliveryType == DeliveryType.ToAddress)
            {
                if (inputModel.UserNeighborhood == null ||
                    inputModel.UserStreet == null ||
                    inputModel.UserStreetNumber == null ||
                    inputModel.UserFloor == null ||
                    inputModel.UserApartmentNumber == null)
                {
                    return this.RedirectToAction(nameof(this.Create));
                }
            }

            var user = await this.userManager.GetUserAsync(this.User);

            inputModel.UserId = user.Id;
            inputModel.ExpectedDeliveryDate = DateTime.UtcNow.AddDays(3);
            inputModel.DeliveryPrice = GlobalConstants.Order.DeliveryPrice;

            await this.orderService.CreateOrderAsync(inputModel);

            return this.RedirectToAction(nameof(this.Summary));
        }

        public async Task<ActionResult> Summary()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = await this.orderService.GetProcessingOrderAsync<ConfirmOrderViewModel>(user.Id);

            var cartTotalPrice = await this.cartService.GetCartTotalPriceAsync(user.Id);
            viewModel.CartTotalPrice = cartTotalPrice;

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Summary")]
        public async Task<ActionResult> SummaryPost()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            await this.orderService.ConfirmOrderAsync(user.Id);

            this.TempData["success"] = GlobalConstants.Order.OrderCreated;

            return this.RedirectToAction("Index", "Home");
        }
    }
}
