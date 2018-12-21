using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Linq;

namespace SportsStore.Controllers
{
    public class ProductController:Controller
    {
        private readonly IProductRepository _productRepository;
        private int _pageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (_pageSize == value || value < 1) return;

                _pageSize = value;
            }
        }

        public ViewResult List(int productPage = 1) => View
            (
            _productRepository.Products
            .OrderBy(p => p.ProductId)
            .Skip((productPage - 1) * PageSize)
            .Take(PageSize)
            );
    }
}
