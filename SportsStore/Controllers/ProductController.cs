using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;

namespace SportsStore.Controllers
{
    public class ProductController:Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public ViewResult List() => View(_productRepository.Products);
    }
}
