namespace DragonSouvenirs.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using DragonSouvenirs.Common;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Models.Enums;
    using DragonSouvenirs.Services.Data;
    using DragonSouvenirs.Web.ViewModels.Home;
    using DragonSouvenirs.Web.ViewModels.Offices;
    using DragonSouvenirs.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

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

        [Authorize]
        [Route(
            "/MyOrders",
            Name = "myOrdersRoute")]
        public async Task<ActionResult> MyOrders()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var viewModel = new MyOrdersViewModel
            {
                Orders = await this.orderService.GetAllByUserIdAsync<MyOrderViewModel>(userId),
            };

            return this.View(viewModel);
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
                EcontOffices = await this.officeService.GetAllEcontOfficesAsync(),
                SpeedyOffices = await this.officeService.GetAllSpeedyOfficesAsync(),
            };
            offices.Offices = offices.EcontOffices.Concat(offices.SpeedyOffices);

            var cities = new CitiesViewModel
            {
                Cities = await this.officeService.GetAllCitiesAsync(),
            };

            var viewModel = new CreateOrderViewModel
            {
                UserFullName = user.FullName,
                FirstName = user.FullName?.Split(' ').FirstOrDefault(),
                LastName = user.FullName?[user.FullName.IndexOf(" ", StringComparison.Ordinal)..],
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
        public async Task<ActionResult> Create(CreateOrderViewModel inputModel)
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

            var user = await this.userManager.GetUserAsync(this.User);

            inputModel.UserId = user.Id;
            inputModel.ExpectedDeliveryDate = DateTime.UtcNow.AddDays(3);
            inputModel.DeliveryPrice = GlobalConstants.Order.DeliveryPrice;

            await this.orderService.CreateOrderAsync(inputModel);

            if (inputModel.ToUpdateAddress)
            {
                user.FullName = $"{inputModel.FirstName} {inputModel.LastName}";
                user.PhoneNumber = inputModel.InvoiceNumber;
                user.City = inputModel.UserCity;
                user.Neighborhood = inputModel.UserNeighborhood;
                user.Street = inputModel.UserStreet;
                user.StreetNumber = inputModel.UserStreetNumber;
                user.Floor = inputModel.UserFloor;
                user.ApartmentNumber = inputModel.UserApartmentNumber;

                var userDefaultAddress
                    = $"гр. {inputModel.UserCity}, кв. {inputModel.UserNeighborhood}, ул. {inputModel.UserStreet} {inputModel.UserStreetNumber}, ет. {inputModel.UserFloor}, ап. {inputModel.UserApartmentNumber}";

                if (inputModel.UserApartmentBuilding != null)
                {
                    user.ApartmentBuilding = inputModel.UserApartmentBuilding;
                    userDefaultAddress += $", Блок {inputModel.UserApartmentBuilding}";
                }

                if (inputModel.UserEntrance != null)
                {
                    user.Entrance = inputModel.UserEntrance;
                    userDefaultAddress += $", Вход {inputModel.UserEntrance}";
                }

                user.DefaultShippingAddress = userDefaultAddress;
                await userManager.UpdateAsync(user);
            }

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
