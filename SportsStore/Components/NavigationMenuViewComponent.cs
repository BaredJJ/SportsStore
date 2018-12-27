using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Linq;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent:ViewComponent
    {
        private readonly IProductRepository _productRepository;

        public NavigationMenuViewComponent(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(
            _productRepository.Products
            .Select(x => x.Category)
            .Distinct()
            .OrderBy(x => x));
        }
    }
}
