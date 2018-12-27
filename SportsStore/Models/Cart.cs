using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class Cart
    {
        private readonly List<CartLine> _lines;

        public Cart() => _lines = new List<CartLine>();

        public virtual IEnumerable<CartLine> Lines => _lines;

        public virtual void AddItem(Product product, int quantity)
        {
            var line = _lines.Where(p => p.Product.ProductId == product.ProductId).FirstOrDefault();

            if (line == null)
            {
                _lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
                line.Quantity += quantity;
        }

        public virtual void RemoveLine(Product product) => _lines.RemoveAll(line => line.Product.ProductId == product.ProductId);

        public virtual decimal ComputeTotalValue() => _lines.Sum(s => s.Product.Price * s.Quantity);

        public virtual void Clear() => _lines.Clear();     
    }
}
