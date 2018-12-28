using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Linq;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController:Controller
    {
        private readonly IProductRepository _productRepository;

        public CartController(IProductRepository productRepository) => 
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));

        public ViewResult Index(string returnUrl) => View(new CartIndexViewModel
        {
            Cart = GetCart(),
            ReturnUrl = returnUrl
        });

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            var product = GetProduct(productId);
            if(product != null)
            {
                var cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            var product = GetProduct(productId);
            if(product != null)
            {
                var cart = GetCart();
                cart.RemoveLine(product);
                SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        private Product GetProduct(int productId) => _productRepository.Products.FirstOrDefault(p => p.ProductId == productId);

        private Cart GetCart() => HttpContext.Session.GetJSon<Cart>("Cart") ?? new Cart();

        private void SaveCart(Cart cart) => HttpContext.Session.SetJSon("Cart", cart);
    }
}
