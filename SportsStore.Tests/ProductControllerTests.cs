using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Theory]
        [InlineData("Cat1")]
        [InlineData("Cat2")]
        [InlineData("Cat3")]
        private void List_ValidArgument_SpecificProductCount(string categoryName)
        {
            var count = GetProductWithCategory().Count(p => p.Category == categoryName);
            var controller = GetProductControllerWithCategory();

            var result = (controller.List(categoryName).ViewData.Model as ProductsListViewModel).PagingInfo.TotalItems;

            Assert.Equal(result, count);
        }

        [Fact]
        private void List_ValidArgument_CanFilterProductsSameLength()
        {
            const string category = "Cat2";
            var lenght = GetProductWithCategory().Where(p => p.Category == category).Count();
            var controller = GetProductControllerWithCategory();

            var result = (controller.List(category, 1).ViewData.Model as ProductsListViewModel).Products.ToArray().Length;

            Assert.True(result == lenght);
        }

        [Fact]
        private void List_ValidArgument_CanFilterProductsSameValues()
        {
            const string category = "Cat2";
            var values = GetProductWithCategory().Where(p => p.Category == category).ToArray();
            var controller = GetProductControllerWithCategory();

            var result = (controller.List(category, 1).ViewData.Model as ProductsListViewModel).Products.ToArray();

            Assert.True(
                values[0].Name == result[0].Name && values[0].Category == result[0].Category
                &&
                values[1].Name == result[1].Name && values[1].Category == result[1].Category
                );
        }

        [Fact]
        public void List_ValidArgument_ReturnCountElement()
        {
            var controller = GetProductController();
            var standardLength = 2;
            controller.PageSize = 3;

            var result = (controller.List(null, 2).ViewData.Model as ProductsListViewModel).Products;
            var lenght = result.Count();


            Assert.True(lenght == standardLength);
        }

        [Fact]
        public void List_ValidArgument_ReturnSameElements()
        {
            var controller = GetProductController();
            var collection = GetProducts().ToArray();
            var standardNames = new string[] { collection[3].Name, collection[4].Name };

            var result = (controller.List(null, 2).ViewData.Model as ProductsListViewModel).Products.ToArray();

            Assert.True(
                result[0].Name == standardNames[0] 
                && 
                result[1].Name == standardNames[1]
                );
        }

        [Fact]
        public void List_ValidArgument_CanSendPaginationViewModel()
        {
            var controller = GetProductController();

            var result = (controller.List(null, 2).ViewData.Model as ProductsListViewModel).PagingInfo;

            Assert.True
                (
                    result.CurrentPage == 2 
                    &&
                    result.ItemsPerPage == 3
                    &&
                    result.TotalItems == 5
                    &&
                    result.TotalPages == 2
                );
        }

        private IQueryable<Product> GetProductWithCategory() => GetterFakeRepository.GetProductWithCategory();

        private IQueryable<Product> GetProducts() => GetterFakeRepository.GetProducts();


        private IProductRepository GetProductRepository(IQueryable<Product> products) => GetterFakeRepository.GetProductRepository(products);

        private ProductController GetProductControllerWithCategory() => 
            new ProductController(GetProductRepository(GetProductWithCategory())) { PageSize = 3 };

        private ProductController GetProductController() => new ProductController(GetProductRepository(GetProducts())) { PageSize = 3 };
    }
}
