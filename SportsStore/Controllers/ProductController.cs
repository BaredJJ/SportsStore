using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
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

        public ViewResult List(string category, int productPage = 1) => View
            (
                new ProductsListViewModel
                {
                    Products = _productRepository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductId)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),

                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = category == null ?
                        _productRepository.Products.Count():
                        _productRepository.Products.Where(e => e.Category == category).Count()
                    },
                    CurrentCategory = category
                }
            );
    }
}
