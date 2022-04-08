namespace DragonSouvenirs.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Repositories;
    using DragonSouvenirs.Services.Data.Common;
    using DragonSouvenirs.Services.Data.Tests.Common;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels;
    using DragonSouvenirs.Web.ViewModels.Products;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = ".")]
    public class ProductsServiceTests
    {
        private readonly EfDeletableEntityRepository<Product> productRepository;
        private readonly EfDeletableEntityRepository<ProductCategory> productCategoryRepository;
        private readonly EfDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly EfDeletableEntityRepository<FavouriteProduct> favouriteProductRepository;

        private readonly CategoriesService categoriesService;
        private readonly EfDeletableEntityRepository<Category> categoryRepository;

        private readonly ProductsService productsService;

        public ProductsServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
            var context = TestDbContextInit.Init();

            this.categoryRepository = new EfDeletableEntityRepository<Category>(context);
            this.productRepository = new EfDeletableEntityRepository<Product>(context);
            this.productCategoryRepository = new EfDeletableEntityRepository<ProductCategory>(context);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            this.favouriteProductRepository = new EfDeletableEntityRepository<FavouriteProduct>(context);
            var cloudinaryAccount = new Account(CloudinaryConfig.CloudName, CloudinaryConfig.ApiKey,
                CloudinaryConfig.ApiSecret);
            var cloudinary = new Cloudinary(cloudinaryAccount);

            this.categoriesService = new CategoriesService(this.categoryRepository, productRepository, cloudinary);
            this.productsService = new ProductsService(this.productRepository, this.productCategoryRepository,
                this.favouriteProductRepository, this.categoriesService, cloudinary);
        }

        [Fact]
        public async Task GetAllAdminAsync()
        {
            await this.SeedData();

            var result = await this.productsService.GetAllAdminAsync<SimpleProductViewModel>();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetDeletedAsync()
        {
            await this.SeedData();

            var result = await this.productsService.GetDeletedAsync<SimpleProductViewModel>();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAsync()
        {
            await this.SeedData();

            var result = await this.productsService.GetAllAsync<SimpleProductViewModel>(1, 0);

            Assert.Equal("Name3", result.First().Name);
        }

        [Theory]
        [InlineData(21, 31, 2)]
        [InlineData(1, 1, 0)]
        public async Task GetAllAsyncWithMinPriceMaxPrice(int minPrice, int maxPrice, int expectedCount)
        {
            await this.SeedData();

            var result = await this.productsService.GetAllAsync<SimpleProductViewModel>(5, 0, minPrice: minPrice, maxPrice: maxPrice);

            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [InlineData("Name2", 1)]
        [InlineData(null, 0)]
        public async Task GetAllByCategoryNameAsync(string categoryName, int expectedCount)
        {
            await this.SeedData();

            var result = await this.productsService.GetAllByCategoryNameAsync<SimpleProductViewModel>(categoryName, 5, 0);

            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [InlineData("Name2", 21, 31, 1)]
        [InlineData(null, 1, 1, 0)]
        public async Task GetAllByCategoryNameAsyncWithMinPriceMaxPrice(string categoryName, int minPrice, int maxPrice, int expectedCount)
        {
            await this.SeedData();

            var result = await this.productsService.GetAllByCategoryNameAsync<SimpleProductViewModel>(categoryName, 5, 0, minPrice: minPrice, maxPrice: maxPrice);

            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public async Task GetCountByCategoryIdAsync()
        {
            await this.SeedData();

            var result = await this.productsService.GetCountByCategoryIdAsync(2);

            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData(2, 21, 31, 1)]
        [InlineData(-1, 1, 1, 0)]
        public async Task GetCountByCategoryIdAsyncWithMinPriceMaxPrice(int categoryId, int minPrice, int maxPrice, int expectedCount)
        {
            await this.SeedData();

            var result = await this.productsService.GetCountByCategoryIdAsync(categoryId, minPrice, maxPrice);

            Assert.Equal(expectedCount, result);
        }

        [Fact]
        public async Task GetTopDiscountedItems()
        {
            await this.SeedData();

            var result = await this.productsService.GetTopDiscountedItems<SimpleProductViewModel>();

            Assert.Equal("Name3", result.First().Name);
        }

        [Fact]
        public async Task GetFavouriteProductsAsync()
        {
            await this.SeedData();
            var user = await this.userRepository.All().FirstOrDefaultAsync();

            var result = await this.productsService.GetFavouriteProductsAsync<FavouriteProductViewModel>(user.Id);

            Assert.Equal(2, result.Count());
        }

        [Theory]
        [InlineData("Name5", true)]
        [InlineData("InvalidProduct", false)]
        public async Task FavouriteProductAsync(string productName, bool expectedResult)
        {
            await this.SeedData();
            var user = await this.userRepository.All().FirstOrDefaultAsync();

            var result = await this.productsService.FavouriteProductAsync(user.Id, productName);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(0, 1000, 3)]
        [InlineData(25, 30, 2)]
        public async Task GetCountAsync(int minPrice, int maxPrice, int expectedResult)
        {
            await this.SeedData();

            var result = await this.productsService.GetCountAsync(minPrice, maxPrice);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(-1, null)]
        [InlineData(5, "Name5")]
        public async Task GetByIdAsync(int productId, string expectedResult)
        {
            await this.SeedData();

            var result = await this.productsService.GetByIdAsync<SimpleProductViewModel>(productId);

            Assert.Equal(expectedResult, result?.Name);
        }

        [Theory]
        [InlineData("InvalidProductName", null)]
        [InlineData("Name5", "Name5")]
        public async Task GetByNameAsync(string productName, string expectedResult)
        {
            await this.SeedData();

            var result = await this.productsService.GetByNameAsync<SimpleProductViewModel>(productName);

            Assert.Equal(expectedResult, result?.Name);
        }

        [Theory]
        [InlineData(-1, null)]
        [InlineData(1, "Name1")]
        public async Task AdminGetByIdAsync(int? productId, string expectedResult)
        {
            await this.SeedData();

            var result = await this.productsService.AdminGetByIdAsync<SimpleProductViewModel>(productId);

            Assert.Equal(expectedResult, result?.Name);
        }

        [Theory]
        [InlineData(-1, null)]
        [InlineData(1, "Title1")]
        public async Task DeleteRecoverAsync(int productId, string expectedResult)
        {
            await this.SeedData();

            var result = await this.productsService.DeleteRecoverAsync(productId);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task MostExpensiveProductPrice()
        {
            await this.SeedData();

            var result = await this.productsService.MostExpensiveProductPrice();

            Assert.Equal(30, result);
        }

        [Fact]
        public async Task LeastExpensiveProductPrice()
        {
            await this.SeedData();

            var result = await this.productsService.LeastExpensiveProductPrice();

            Assert.Equal(2, result);
        }

        private async Task SeedData()
        {
            // Seed Categories [1-DELETED,2,3]
            for (int i = 0; i < 3; i++)
            {
                await this.categoryRepository.AddAsync(
                    new Category
                    {
                        Id = i + 1,
                        Name = $"Name{i + 1}",
                        Title = $"Title{i + 1}",
                        Content = $"Content{i + 1}",
                        ImageUrl = $"ImgUrl{i + 1}",
                        IsDeleted = i == 0,
                        DeletedOn = i == 0 ? DateTime.UtcNow : null,
                    });
            }

            // Seed Products [1-DELETED,2-DELETED+DISCOUNT,3-DISCOUNT,4,5]
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
                        Quantity = i + 1,
                        Height = i + 1,
                        Width = i + 1,
                        IsDeleted = i < 2,
                        DeletedOn = i < 2 ? DateTime.UtcNow : null,
                    });
            }

            // Seed ProductsCategories [1-1, 2-2, 3-3, 4-1, 5-2]
            for (int i = 0; i < 5; i++)
            {
                await this.productCategoryRepository.AddAsync(new ProductCategory
                {
                    ProductId = i + 1,
                    CategoryId = (i % 3) + 1,
                });
            }

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

            // Seed FavouriteProducts [1-2, 1-3, 1-4]
            for (int i = 1; i < 4; i++)
            {
                await this.favouriteProductRepository.AddAsync(new FavouriteProduct
                {
                    UserId = user.Id,
                    ProductId = i + 1,
                });
            }

            await this.categoryRepository.SaveChangesAsync();
            await this.productRepository.SaveChangesAsync();
            await this.productCategoryRepository.SaveChangesAsync();
            await this.favouriteProductRepository.SaveChangesAsync();
        }
    }
}
