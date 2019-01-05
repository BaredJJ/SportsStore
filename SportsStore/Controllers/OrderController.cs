using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController:Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Cart _cart;

        public OrderController(IOrderRepository orderRepository, Cart cartService)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _cart = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if(!_cart.Lines.Any())
                ModelState.AddModelError("", "Sorry your cart is empty!");

            if (!ModelState.IsValid) return View(order);

            order.Lines = _cart.Lines.ToArray();
            _orderRepository.SaveOrder(order);
            return RedirectToAction(nameof(Completed));

        }

        public ViewResult Completed()
        {
            _cart.Clear();
            return View();
        }

        public ViewResult List() => View(_orderRepository.Orders.Where(o => !o.Shipped));

        [HttpPost]
        public IActionResult MarkShipped(int orderId)
        {
            var order = _orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                order.Shipped = true;
                _orderRepository.SaveOrder(order);
            }

            return RedirectToAction(nameof(List));
        }
    }
}
