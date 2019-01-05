using Moq;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Tests
{
    public class GetterFakeRepository
    {

        public static IProductRepository GetProductRepository(IQueryable<Product> products)
        {
            return GetMockIProductRepository(products).Object;
        }

        public static Mock<IProductRepository> GetMockIProductRepository(IQueryable<Product> products)
        {
            var repository = new Mock<IProductRepository>();
            repository.Setup(m => m.Products).Returns(products);
            return repository;
        }

        public static IQueryable<Product> GetProductWithCategory()
        {
            var products = GetProducts();

            products.ElementAt(0).Category = "Cat1";
            products.ElementAt(1).Category = "Cat2";
            products.ElementAt(2).Category = "Cat1";
            products.ElementAt(3).Category = "Cat2";
            products.ElementAt(4).Category = "Cat3";

            return products;
        }

        public static IQueryable<Product> GetProducts() => new Product[]
{
            new Product { ProductId = 1, Name = "P1", Price = 100M},
            new Product { ProductId = 2, Name = "P2", Price = 50M },
            new Product { ProductId = 3, Name = "P3", Price = 20M},
            new Product { ProductId = 4, Name = "P4", Price = 30M},
            new Product { ProductId = 5, Name = "P5", Price = 200M}
        }.AsQueryable();
    }
}
