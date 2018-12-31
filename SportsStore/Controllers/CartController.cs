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
        private readonly Cart _cart;

        public CartController(IProductRepository productRepository, Cart cartService)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _cart = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }


        public ViewResult Index(string returnUrl) => View(new CartIndexViewModel
        {
            Cart = _cart,
            ReturnUrl = returnUrl
        });

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            var product = GetProduct(productId);
            if(product != null)
            {
                _cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            var product = GetProduct(productId);
            if(product != null)
            {
                _cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        private Product GetProduct(int productId) => _productRepository.Products.FirstOrDefault(p => p.ProductId == productId);
    }
}
