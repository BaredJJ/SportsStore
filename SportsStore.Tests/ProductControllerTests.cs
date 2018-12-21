using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
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

        private ProductController GetProductController() => new ProductController(GetProductRepository());

        [Fact]
        public void List_ValidArgument_ReturnCountElement()
        {
            var controller = GetProductController();
            var standardLength = 2;
            controller.PageSize = 3;

            var result = controller.List(2).ViewData.Model as IEnumerable<Product>;
            var lenght = result.Count();


            Assert.True(lenght == standardLength);
        }

        [Fact]
        public void List_ValidArgument_ReturnSameElements()
        {
            var controller = GetProductController();
            var collection = GetProductRepository().Products.ToArray();
            var standardNames = new string[] { collection[3].Name, collection[4].Name };
            controller.PageSize = 3;

            var result = (controller.List(2).ViewData.Model as IEnumerable<Product>).ToArray();

            Assert.True(
                result[0].Name == standardNames[0] 
                && 
                result[1].Name == standardNames[1]
                );
        }
    }
}
