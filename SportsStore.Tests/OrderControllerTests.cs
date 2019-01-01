using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void CheckoutPost_EmptyCart_NotSaveShippingDetails()
        {
            var order = GetOrder();
            var repository = GetOrderRepository();
            var target = GetOrderController(repository.Object);

            var result = target.Checkout(order) as ViewResult;

            repository.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public void CheckoutPost_EmptyCart_ReturnDefaultView()
        {
            var order = GetOrder();
            var repository = GetOrderRepository();
            var target = GetOrderController(repository.Object);

            var result = target.Checkout(order) as ViewResult;

            Assert.True(string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public void CheckoutPost_EmptyCart_ModelIsNotValid()
        {
            var order = GetOrder();
            var repository = GetOrderRepository();
            var target = GetOrderController(repository.Object);

            var result = target.Checkout(order) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void CheckoutPost_InvalidShippingDetails_NotSaveShippingDetails()
        {
            var order = GetOrder();
            var repository = GetOrderRepository();
            var target = GetFillOrderController(repository.Object);
            target.ModelState.AddModelError("error", "error");

            var result = target.Checkout(order) as ViewResult;

            repository.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public void CheckoutPost_InvalidShippingDetails_ReturnDefaultView()
        {
            var order = GetOrder();
            var repository = GetOrderRepository();
            var target = GetFillOrderController(repository.Object);
            target.ModelState.AddModelError("error", "error");

            var result = target.Checkout(order) as ViewResult;

            Assert.True(string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public void CheckoutPost_InvalidShippingDetails_ModelIsNotValid()
        {
            var order = GetOrder();
            var repository = GetOrderRepository();
            var target = GetFillOrderController(repository.Object);
            target.ModelState.AddModelError("error", "error");

            var result = target.Checkout(order) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void CheckoutPost_ValidArgument_SaveShippingDetails()
        {
            var order = GetOrder();
            var repository = GetOrderRepository();
            var target = GetFillOrderController(repository.Object);

            var result = target.Checkout(order) as RedirectToActionResult;

            repository.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public void CheckoutPost_ValidArgument_SubmitOrder()
        {
            var order = GetOrder();
            var repository = GetOrderRepository();
            var target = GetFillOrderController(repository.Object);

            var result = target.Checkout(order) as RedirectToActionResult;

            Assert.Equal("Completed", result.ActionName);
        }

        private OrderController GetOrderController(IOrderRepository orderRepository) => new OrderController(orderRepository, GetCart());

        private OrderController GetFillOrderController(IOrderRepository orderRepository) => new OrderController(orderRepository, GetFillCart());

        private Order GetOrder() => new Order();

        private Cart GetCart() => new Cart();

        private Cart GetFillCart()
        {
            var cart = new Cart();
            cart.AddItem(new Product(), 1);
            return cart;
        }

        private Mock<IOrderRepository> GetOrderRepository() => new Mock<IOrderRepository>();
    }
}
