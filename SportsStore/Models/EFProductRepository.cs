using System;
using System.Linq;

namespace SportsStore.Models
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EFProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IQueryable<Product> Products => _dbContext.Products;

        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
                _dbContext.Products.Add(product);
            else
            {
                var dbEntry = _dbContext.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
            }

            _dbContext.SaveChanges();
        }

        public Product DeleteProduct(int productId)
        {
            var dbEntry = _dbContext.Products.FirstOrDefault(p => p.ProductId == productId);

            if (dbEntry == null) return null;

            _dbContext.Products.Remove(dbEntry);
            _dbContext.SaveChanges();
            return dbEntry;
        }
    }
}
