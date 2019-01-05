using SportsStore.Models;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Theory]
        [InlineData(0, 1, 2, 1)]
        public void Add_ValidArgument_AddNewLines(int firstIndex, int secondIndex, int length, int quantity)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var target = GetCart();
            
            target.AddItem(p1, quantity);
            target.AddItem(p2, quantity);
            var results = target.Lines.Count();

            Assert.True(results == length);

        }

        [Theory]
        [InlineData(0, 1, 1)]
        public void Add_ValidArgument_SameValues(int firstIndex, int secondIndex, int quantity)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var target = GetCart();

            target.AddItem(p1, quantity);
            target.AddItem(p2, quantity);
            var results = target.Lines.ToArray();

            Assert.True(results[0].Product == p1 && results[1].Product == p2);

        }

        [Theory]
        [InlineData(0, 1, 2, 1, 10)]
        public void Add_ValidArgument_AddNewLinesDifferentQuantity(int firstIndex, int secondIndex, int length, int firstQuantity, int secondQuantity)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var target = GetCart();

            target.AddItem(p1, firstQuantity);
            target.AddItem(p2, firstQuantity);
            target.AddItem(p1, secondQuantity);
            var results = target.Lines.Count();

            Assert.True(results == length);

        }

        [Theory]
        [InlineData(0, 1, 1, 10)]
        public void Add_ValidArgument_SameValuesDifferentQuantity(int firstIndex, int secondIndex, int firstQuantity, int secondQuantity)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var target = GetCart();

            target.AddItem(p1, firstQuantity);
            target.AddItem(p2, firstQuantity);
            target.AddItem(p1, secondQuantity);
            var results = target.Lines.ToArray();

            Assert.True(results[0].Quantity == secondQuantity + firstQuantity && results[1].Quantity == firstQuantity);
        }

        [Theory]
        [InlineData(0, 1, 3, 2)]
        public void Remove_ValidArgument_SameLengthAfterRemove(int firstIndex, int secondIndex, int thirdIndex , int length)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var p3 = products.ElementAt(thirdIndex);
            var target = GetCart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            target.RemoveLine(p2);

            var results = target.Lines.Count();

            Assert.True(results == length);
        }

        [Theory]
        [InlineData(0, 1, 3, 0)]
        public void Remove_ValidArgument_RemoveLine(int firstIndex, int secondIndex, int thirdIndex , int length)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var p3 = products.ElementAt(thirdIndex);
            var target = GetCart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            target.RemoveLine(p2);

            var results = target.Lines.Count(p => p.Product.ProductId == p2.ProductId);

            Assert.True(results == length);
        }

        [Theory]
        [InlineData(0, 1, 1, 3)]
        public void ComputeTotalValue_ValidArgument_CalculateCartTotal(int firstIndex, int secondIndex, int firstQuantity, int secondQuantity)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var sum = p1.Price * firstQuantity + p2.Price * secondQuantity;
            var target = GetCart();

            target.AddItem(p1, firstQuantity);
            target.AddItem(p2, secondQuantity);

            var results = target.ComputeTotalValue();

            Assert.True(results == sum);
        }

        [Theory]
        [InlineData(0, 1, 0, 1)]
        public void Clear_ValidArgument_ClearContext(int firstIndex, int secondIndex, int length, int quantity)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var target = GetCart();

            target.AddItem(p1, quantity);
            target.AddItem(p2, quantity);

            target.Clear();
            var results = target.Lines.Count();
                

            Assert.True(results == length);
        }

        private IQueryable<Product> GetProducts() => GetterFakeRepository.GetProductWithCategory();

        private Cart GetCart() => new Cart();
    }
}
