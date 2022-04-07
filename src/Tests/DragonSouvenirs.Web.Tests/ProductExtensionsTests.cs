namespace DragonSouvenirs.Web.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Extensions;
    using ViewModels.Categories;
    using Xunit;

    public class ProductExtensionsTests
    {
        private readonly List<ProductInCategoryViewModel> products;

        public ProductExtensionsTests()
        {
            this.products = new List<ProductInCategoryViewModel>
            {
                new ProductInCategoryViewModel
                {
                    Id = 1,
                    Name = "Name1",
                    Title = "Title1 Search",
                    Description = "Description1",
                    Price = 1M,
                    DiscountPrice = 1M,
                    Quantity = 1,
                    Height = 1,
                    Width = 1,
                    Images = new List<ImageViewModel>
                    {
                        new ImageViewModel { ImgUrl = "imageUrl1" },
                    },
                    CategoryName = "CategoryName1",
                    CategoryTitle = "CategoryTitle1",
                },
                new ProductInCategoryViewModel
                {
                    Id = 2,
                    Name = "Name2",
                    Title = "Title2",
                    Description = "Description2",
                    Price = 2M,
                    DiscountPrice = 2M,
                    Quantity = 2,
                    Height = 2,
                    Width = 2,
                    Images = new List<ImageViewModel>
                    {
                        new ImageViewModel { ImgUrl = "imageUrl2" },
                    },
                    CategoryName = "CategoryName2",
                    CategoryTitle = "CategoryTitle2",
                },
            };
        }

        [Theory]
        [InlineData("Title1")]
        [InlineData("Title1,")]
        [InlineData("Title1.")]
        [InlineData("Title1 ")]
        [InlineData("Title1\\")]
        [InlineData("Title1/")]
        [InlineData("Title1|")]
        [InlineData("Title1!")]
        [InlineData("Title1?")]
        [InlineData("Title1?Search")]
        public void FilterBySearchStringShouldFilterOnValidSearchString(string searchString)
        {
            var expectedResult = new List<ProductInCategoryViewModel>
            {
                new ProductInCategoryViewModel
                {
                    Id = 1,
                    Name = "Name1",
                    Title = "Title1 Search",
                    Description = "Description1",
                    Price = 1M,
                    DiscountPrice = 1M,
                    Quantity = 1,
                    Height = 1,
                    Width = 1,
                    Images = new List<ImageViewModel>
                    {
                        new ImageViewModel { ImgUrl = "imageUrl1" },
                    },
                    CategoryName = "CategoryName1",
                    CategoryTitle = "CategoryTitle1",
                },
            }.Select(er => er.Title).ToList();

            var result = this.products
                .FilterBySearchString(searchString)
                .Select(r => r.Title)
                .ToList();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null)]
        public void FilterBySearchStringShouldReturnItemOnNull(string searchString)
        {
            var result = this.products.FilterBySearchString(searchString);

            Assert.Equal(this.products, result);
        }

        [Theory]
        [InlineData("abcde")]
        public void FilterBySearchStringShouldReturnEmptyItem(string searchString)
        {
            var result = this.products.FilterBySearchString(searchString);

            Assert.Empty(result);
        }
    }
}
