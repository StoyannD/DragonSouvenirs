namespace DragonSouvenirs.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using DragonSouvenirs.Data;
    using DragonSouvenirs.Data.Models;
    using DragonSouvenirs.Data.Repositories;
    using DragonSouvenirs.Services.Data.Common;
    using DragonSouvenirs.Services.Mapping;
    using DragonSouvenirs.Web.ViewModels;
    using DragonSouvenirs.Web.ViewModels.Administration.Categories;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = ".")]
    public class CategoriesServiceTests
    {
        private readonly EfDeletableEntityRepository<Category> categoryRepository;
        private readonly ICategoriesService categoriesService;

        public CategoriesServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);

            this.categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var productRepository = new EfDeletableEntityRepository<Product>(context);
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
        public async Task GetByNameAsync()
        {
            await this.SeedData();

            var result = await this.categoriesService.GetByNameAsync<AdminCategoryViewModel>("Name5");

            Assert.Equal("Name5", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync()
        {
            await this.SeedData();

            var result = await this.categoriesService.GetByIdAsync<AdminCategoryViewModel>(5);

            Assert.Equal(5, result.Id);
        }

        [Fact]
        public async Task DeleteRecoverAsync()
        {
            await this.SeedData();

            var result = await this.categoriesService.DeleteRecoverAsync(1);

            Assert.Equal("Title1", result);
        }

        [Fact]
        public async Task DeleteRecoverAsyncNonexistentCategory()
        {
            await this.SeedData();

            var result = await this.categoriesService.DeleteRecoverAsync(23);

            Assert.Null(result);
        }

        private async Task SeedData()
        {
            await this.categoryRepository.AddAsync(
                new Category
                {
                    Id = 1,
                    Name = "Name1",
                    Title = "Title1",
                    Content = "Content1",
                    ImageUrl = "ImgUrl1",
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });
            await this.categoryRepository.AddAsync(
                new Category
                {
                    Id = 2,
                    Name = "Name2",
                    Title = "Title2",
                    Content = "Content2",
                    ImageUrl = "ImgUrl2",
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });
            await this.categoryRepository.AddAsync(
                new Category
                {
                    Id = 3,
                    Name = "Name3",
                    Title = "Title3",
                    Content = "Content3",
                    ImageUrl = "ImgUrl3",
                });
            await this.categoryRepository.AddAsync(
                new Category
                {
                    Id = 4,
                    Name = "Name4",
                    Title = "Title4",
                    Content = "Content4",
                    ImageUrl = "ImgUrl4",
                });
            await this.categoryRepository.AddAsync(
                new Category
                {
                    Id = 5,
                    Name = "Name5",
                    Title = "Title5",
                    Content = "Content5",
                    ImageUrl = "ImgUrl5",
                });
            await this.categoryRepository.SaveChangesAsync();
        }
    }
}
