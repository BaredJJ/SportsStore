using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Delete_ValidArgument_CallDeleteProduct(int productId)
        {
            var mockRepository = GetMockRepository();
            var target = GetConfigureAdminController(mockRepository.Object);

            target.Delete(productId);

           mockRepository.Verify(m => m.DeleteProduct(productId));
        }

        [Fact]
        public void EditPost_BadArgument_NotCallSaveChanges()
        {
            var repository = new Mock<IProductRepository>();
            var target = GetConfigureAdminController(repository.Object);
            var product = new Product { Name = "Test" };
            target.ModelState.AddModelError("error", "error");

            var result = target.Edit(product);

            repository.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
        }

        [Fact]
        public void EditPost_BadArgument_ReturnValueIsViewResult()
        {
            var target = GetAdminController();
            var product = new Product { Name = "Test" };
            target.ModelState.AddModelError("error", "error");

            var result = target.Edit(product);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void EditPost_ValidArgument_CallSaveChanges()
        {
            var repository = new Mock<IProductRepository>();
            var tempData = GetTempData();
            var target = GetConfigureAdminController(repository.Object);
            target.TempData = tempData;
            var product = new Product {Name = "Test"};

            var result = target.Edit(product);

            repository.Verify(m => m.SaveProduct(product));
        }

        [Fact]
        public void EditPost_ValidArgument_ReturnValueIsRedirectToActionResult()
        {
            var tempData = GetTempData();
            var target = GetAdminController();
            target.TempData = tempData;
            var product = new Product {Name = "Test"};

            var result = target.Edit(product);

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void EditPost_ValidArgument_ActionNameIsIndex()
        {
            var tempData = GetTempData();
            var target = GetAdminController();
            target.TempData = tempData;
            var product = new Product {Name = "Test"};

            var result = target.Edit(product);

            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(3, 4)]
        [InlineData(4, 5)]
        public void Edit_ValidArgument_ReturnProduct(int productIndex, int productId)
        {
            var standardProductId = GetProducts().ElementAt(productIndex).ProductId;
            var target = GetAdminController();

            var result = GetViewModel<Product>(target.Edit(productId));

            Assert.Equal(standardProductId, result.ProductId);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(54)]
        [InlineData(6)]
        [InlineData(0)]
        public void Edit_BadArgument_ReturnNull(int productIndex)
        {
            var target = GetAdminController();

            var result = GetViewModel<Product>(target.Edit(productIndex));

            Assert.Null(result);
        }

        [Fact]
        public void Index_ValidArgument_ValidLength()
        {
            var length = GetProducts().ToArray().Length;
            var target = GetAdminController();

            var result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray().Length;

            Assert.True(length == result);
        }

        [Fact]
        public void Index_ValidArgument_ContainsAllProduct()
        {
            var standard = GetProducts().ToArray();
            var target = GetAdminController();

            var result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();

            Assert.True(
                        standard[0].Name == result[0].Name
                                       &&
                        standard[1].Name == result[1].Name
                                       && 
                        standard[2].Name == result[2].Name
                                       && 
                        standard[3].Name == result[3].Name
                                       &&  
                        standard[4].Name == result[4].Name
                        );
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        private AdminController GetAdminController() => new AdminController(GetIProductRepository());

        private AdminController GetConfigureAdminController(IProductRepository repository) => new AdminController(repository);

        private IProductRepository GetIProductRepository() => GetterFakeRepository.GetProductRepository(GetProducts());

        private IQueryable<Product> GetProducts() => GetterFakeRepository.GetProducts();

        private ITempDataDictionary GetTempData() => new Mock<ITempDataDictionary>().Object;

        private Mock<IProductRepository> GetMockRepository() =>
            GetterFakeRepository.GetMockIProductRepository(GetProducts());
    }
}
