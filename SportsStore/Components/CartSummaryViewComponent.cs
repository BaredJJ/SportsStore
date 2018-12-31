using System;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components
{
    public class CartSummaryViewComponent:ViewComponent
    {
        private readonly Cart _cart;

        public CartSummaryViewComponent(Cart cartService)
        {
            _cart = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
