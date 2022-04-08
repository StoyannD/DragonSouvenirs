namespace DragonSouvenirs.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Repositories;
    using DragonSouvenirs.Services.Data.Tests.Common;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels;
    using DragonSouvenirs.Web.ViewModels.Cart;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = ".")]
    public class CartServiceTests
    {
        private readonly EfDeletableEntityRepository<Category> categoryRepository;
        private readonly EfDeletableEntityRepository<ProductCategory> productCategoryRepository;

        private readonly EfDeletableEntityRepository<Cart> cartRepository;
        private readonly EfDeletableEntityRepository<CartProduct> cartProductRepository;
        private readonly EfDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly EfDeletableEntityRepository<Product> productRepository;
        private readonly CartService cartService;

        public CartServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
            var context = TestDbContextInit.Init();

            this.categoryRepository = new EfDeletableEntityRepository<Category>(context);
            this.productCategoryRepository = new EfDeletableEntityRepository<ProductCategory>(context);

            this.cartRepository = new EfDeletableEntityRepository<Cart>(context);
            this.cartProductRepository = new EfDeletableEntityRepository<CartProduct>(context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            this.productRepository = new EfDeletableEntityRepository<Product>(context);

            this.cartService = new CartService(this.cartRepository, cartProductRepository, userRepository, productRepository);
        }

        [Fact]
        public async Task GetCartProductsAsync()
        {
            await this.SeedData();
            var user = await this.userRepository.All().FirstOrDefaultAsync();

            var resultValid = await this.cartService.GetCartProductsAsync<CartProductViewModel>(user.Id);
            var resultInvalid = await this.cartService.GetCartProductsAsync<CartProductViewModel>(null);

            Assert.Equal(3, resultValid.Count());
            Assert.Empty(resultInvalid);
        }

        [Theory]
        [InlineData(0, 570)]
        [InlineData(10, 513)]
        public async Task GetCartTotalPriceAsync(decimal discountPercentage, decimal expectedResult)
        {
            await this.SeedData();
            var user = await this.userRepository.All().FirstOrDefaultAsync();

            var result = await this.cartService.GetCartTotalPriceAsync(user.Id, discountPercentage);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 10, false)]
        [InlineData(3, 10, true)]
        [InlineData(3, 200, false)]
        [InlineData(-1, 200, false)]
        public async Task AddProductToCartAsync(int productId, int quantity, bool expectedResult)
        {
            await this.SeedData();
            var user = await this.userRepository.All().FirstOrDefaultAsync();

            var result = await this.cartService.AddProductToCartAsync(user.Id, productId, quantity);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(3, true)]
        [InlineData(-1, false)]
        public async Task DeleteProductFromCartAsync(int productId, bool expectedResult)
        {
            await this.SeedData();
            var user = await this.userRepository.All().FirstOrDefaultAsync();

            var result = await this.cartService.DeleteProductFromCartAsync(user.Id, productId);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 10, false)]
        [InlineData(3, 10, true)]
        [InlineData(3, 200, false)]
        [InlineData(-1, 200, false)]
        public async Task EditProductInCartAsync(int productId, int quantity, bool expectedResult)
        {
            await this.SeedData();
            var user = await this.userRepository.All().FirstOrDefaultAsync();

            var result = await this.cartService.EditProductInCartAsync(user.Id, productId, quantity);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task GetCartByIdAsync()
        {
            await this.SeedData();
            var user = await this.userRepository.All().FirstOrDefaultAsync();

            var resultValid = await this.cartService.GetCartByIdAsync<SimpleCartViewModel>(user.Id);
            var resultInvalid = await this.cartService.GetCartByIdAsync<SimpleCartViewModel>(null);

            Assert.Equal(1, resultValid.Id);
            Assert.Null(resultInvalid);
        }

        [Fact]
        public async Task UserHasProductsInCart()
        {
            await this.SeedData();
            var user = await this.userRepository.All().FirstOrDefaultAsync();

            var resultValid = await this.cartService.UserHasProductsInCart(user.Id);
            var resultInvalid = await this.cartService.UserHasProductsInCart(null);

            Assert.True(resultValid);
            Assert.False(resultInvalid);
        }

        private async Task SeedData()
        {
            // Seed User
            await this.userRepository.AddAsync(new ApplicationUser
            {
                FullName = "FullName",
                DefaultShippingAddress = "DefaultShippingAddress",
                City = "City",
                Neighborhood = "Neighborhood",
                Street = "Street",
                StreetNumber = 1,
                ApartmentBuilding = "A",
                Entrance = "1",
                Floor = 1,
                ApartmentNumber = 1,
            });
            await this.userRepository.SaveChangesAsync();
            var user = await this.userRepository.All().FirstOrDefaultAsync();

            // Seed Categories
            await this.categoryRepository.AddAsync(
                new Category
                {
                    Id = 1,
                    Name = "Name1",
                    Title = "Title1",
                    Content = "Content1",
                    ImageUrl = "ImgUrl1",
                });

            // Seed Products
            for (int i = 0; i < 5; i++)
            {
                await this.productRepository.AddAsync(
                    new Product()
                    {
                        Id = i + 1,
                        Name = $"Name{i + 1}",
                        Title = $"Title{i + 1}",
                        Description = $"Description{i + 1}",
                        Price = (i * 5) + 10, // 10 15 20, 25, 30
                        DiscountPrice = i == 2 ? 2 : null,
                        Quantity = 100,
                        Height = i + 1,
                        Width = i + 1,
                        IsDeleted = i < 2,
                        DeletedOn = i < 2 ? DateTime.UtcNow : null,
                    });
            }

            // Seed ProductsCategories
            for (int i = 0; i < 5; i++)
            {
                await this.productCategoryRepository.AddAsync(new ProductCategory
                {
                    ProductId = i + 1,
                    CategoryId = 1,
                });
            }

            // Seed Cart
            await this.cartRepository.AddAsync(new Cart
            {
                Id = 1,
                UserId = user.Id,
            });

            // Seed CartProducts
            for (int i = 0; i < 5; i++)
            {
                await this.cartProductRepository.AddAsync(new CartProduct
                {
                    CartId = 1,
                    ProductId = i + 1,
                    Quantity = 10,
                });
            }

            await this.categoryRepository.SaveChangesAsync();
            await this.productRepository.SaveChangesAsync();
            await this.productCategoryRepository.SaveChangesAsync();
            await this.cartRepository.SaveChangesAsync();
            await this.cartProductRepository.SaveChangesAsync();
        }
    }
}
