using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {

        [Fact]
        public void List_ValidArgument_ReturnCountElement()
        {
            var controller = GetProductController();
            var standardLength = 2;
            controller.PageSize = 3;

            var result = (controller.List(2).ViewData.Model as ProductsListViewModel).Products;
            var lenght = result.Count();


            Assert.True(lenght == standardLength);
        }

        [Fact]
        public void List_ValidArgument_ReturnSameElements()
        {
            var controller = GetProductController();
            var collection = GetProductRepository().Products.ToArray();
            var standardNames = new string[] { collection[3].Name, collection[4].Name };

            var result = (controller.List(2).ViewData.Model as ProductsListViewModel).Products.ToArray();

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

            var result = (controller.List(2).ViewData.Model as ProductsListViewModel).PagingInfo;

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

        private IQueryable<Product> GetProducts() => new Product[]
        {
            new Product { ProductId = 1, Name = "P1"},
            new Product { ProductId = 2, Name = "P2"},
            new Product { ProductId = 3, Name = "P3"},
            new Product { ProductId = 4, Name = "P4"},
            new Product { ProductId = 5, Name = "P5"}
        }.AsQueryable();

        private IProductRepository GetProductRepository()
        {
            var repository = new Mock<IProductRepository>();
            repository.Setup(m => m.Products).Returns(GetProducts);
            return repository.Object;
        }

        private ProductController GetProductController() => new ProductController(GetProductRepository()) { PageSize = 3 };
    }
}
