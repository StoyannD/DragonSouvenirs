namespace DragonSouvenirs.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
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
    using DragonSouvenirs.Web.ViewModels.Administration.Categories;

    using Xunit;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = ".")]
    public class CategoriesServiceTests
    {
        private readonly EfDeletableEntityRepository<Category> categoryRepository;
        private readonly EfDeletableEntityRepository<Product> productRepository;
        private readonly ICategoriesService categoriesService;

        public CategoriesServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
            var context = TestDbContextInit.Init();

            this.categoryRepository = new EfDeletableEntityRepository<Category>(context);
            this.productRepository = new EfDeletableEntityRepository<Product>(context);
            var cloudinaryAccount = new Account(CloudinaryConfig.CloudName, CloudinaryConfig.ApiKey, CloudinaryConfig.ApiSecret);
            var cloudinary = new Cloudinary(cloudinaryAccount);

            this.categoriesService = new CategoriesService(this.categoryRepository, productRepository, cloudinary);
        }

        [Fact]
        public async Task GetAllAsyncTest()
        {
            await this.SeedData();

            var result = await this.categoriesService.GetAllAsync<AdminCategoryViewModel>();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetDeletedAsync()
        {
            await this.SeedData();

            var result = await this.categoriesService.GetDeletedAsync<AdminCategoryViewModel>();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAdminAsync()
        {
            await this.SeedData();

            var result = await this.categoriesService.GetAllAdminAsync<AdminCategoryViewModel>();

            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task GetAllByProductIdAsync()
        {
            await this.SeedData();

            var resultValid = await this.categoriesService.GetAllByProductIdAsync<AdminCategoryViewModel>(1);
            var resultInvalid = await this.categoriesService.GetAllByProductIdAsync<AdminCategoryViewModel>(1234);

            Assert.Equal("Name1", resultValid.First().Name);
            Assert.Empty(resultInvalid);
        }

        [Fact]
        public async Task GetByNameAsync()
        {
            await this.SeedData();

            var resultValid = await this.categoriesService.GetByNameAsync<AdminCategoryViewModel>("Name5");
            var resultInvalid = await this.categoriesService.GetByNameAsync<AdminCategoryViewModel>("InvalidName");

            Assert.Equal("Name5", resultValid.Name);
            Assert.Null(resultInvalid);
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            await this.SeedData();

            var resultValid = await this.categoriesService.GetByIdAsync<AdminCategoryViewModel>(5);
            var resultInvalid = await this.categoriesService.GetByIdAsync<AdminCategoryViewModel>(-1);

            Assert.Equal(5, resultValid.Id);
            Assert.Null(resultInvalid);
        }

        [Fact]
        public async Task DeleteRecoverAsync()
        {
            await this.SeedData();

            var resultValid = await this.categoriesService.DeleteRecoverAsync(1);
            var resultInvalid = await this.categoriesService.DeleteRecoverAsync(23);

            Assert.Equal("Title1", resultValid);
            Assert.Null(resultInvalid);
        }

        private async Task SeedData()
        {
            // Seed Categories [1-DELETED,2-DELETED,3,4,5]
            for (int i = 0; i < 5; i++)
            {
                await this.categoryRepository.AddAsync(
                    new Category
                    {
                        Id = i + 1,
                        Name = $"Name{i + 1}",
                        Title = $"Title{i + 1}",
                        Content = $"Content{i + 1}",
                        ImageUrl = $"ImgUrl{i + 1}",
                        IsDeleted = i < 2,
                        DeletedOn = i < 2 ? DateTime.UtcNow : null,
                    });
            }

            // Seed Products
            await this.productRepository.AddAsync(
                new Product
                {
                    Id = 1,
                    Name = "Name1",
                    Title = "Title1",
                    Description = "Description1",
                    Price = 1M,
                    DiscountPrice = null,
                    Quantity = 1,
                    Height = 1,
                    Width = 1,
                    IsDeleted = false,
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            CategoryId = 1, ProductId = 1,
                        },
                    },
                });

            await this.categoryRepository.SaveChangesAsync();
            await this.productRepository.SaveChangesAsync();
        }
    }
}
