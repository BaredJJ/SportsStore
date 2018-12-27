﻿using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Theory]
        [InlineData(0, 1, 2, 1)]
        public void Add_ValidArgumetn_AddNewLines(int firstIndex, int secondIndex, int lenght, int quantity)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var target = GetCart();

            target.AddItem(p1, quantity);
            target.AddItem(p2, quantity);
            var reaults = target.Lines.Count();

            Assert.True(reaults == lenght);

        }

        [Theory]
        [InlineData(0, 1, 1)]
        public void Add_ValidArgumetn_SameValues(int firstIndex, int secondIndex, int quantity)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var target = GetCart();

            target.AddItem(p1, quantity);
            target.AddItem(p2, quantity);
            var reaults = target.Lines.ToArray();

            Assert.True(reaults[0].Product == p1 && reaults[1].Product == p2);

        }

        [Theory]
        [InlineData(0, 1, 2, 1, 10)]
        public void Add_ValidArgumetn_AddNewLinesDifferntQuantity(int firstIndex, int secondIndex, int lenght, int firstQuantity, int secondQuantity)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var target = GetCart();

            target.AddItem(p1, firstQuantity);
            target.AddItem(p2, firstQuantity);
            target.AddItem(p1, secondQuantity);
            var reaults = target.Lines.Count();

            Assert.True(reaults == lenght);

        }

        [Theory]
        [InlineData(0, 1, 1, 10)]
        public void Add_ValidArgumetn_SameValuesDifferntQuantity(int firstIndex, int secondIndex, int firstQuantity, int secondQuantity)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var target = GetCart();

            target.AddItem(p1, firstQuantity);
            target.AddItem(p2, firstQuantity);
            target.AddItem(p1, secondQuantity);
            var reaults = target.Lines.ToArray();

            Assert.True(reaults[0].Quantity == secondQuantity + firstQuantity && reaults[1].Quantity == firstQuantity);
        }

        [Theory]
        [InlineData(0, 1, 3, 2)]
        public void Remove_ValidArgumetn_SameLenghtAfterRemove(int firstIndex, int secondIndex, int thirdIndex , int lenght)
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

            var reaults = target.Lines.Count();

            Assert.True(reaults == lenght);
        }

        [Theory]
        [InlineData(0, 1, 3, 0)]
        public void Remove_ValidArgumetn_RemoveLine(int firstIndex, int secondIndex, int thirdIndex , int lenght)
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

            var reaults = target.Lines.Count(p => p.Product.ProductId == p2.ProductId);

            Assert.True(reaults == lenght);
        }

        [Theory]
        [InlineData(0, 1, 1, 3)]
        public void ComputeTotalValue_ValidArgumetn_CalculateCartTotal(int firstIndex, int secondIndex, int firstQuantity, int secondQuantity)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var sum = p1.Price * firstQuantity + p2.Price * secondQuantity;
            var target = GetCart();

            target.AddItem(p1, firstQuantity);
            target.AddItem(p2, secondQuantity);

            var reaults = target.ComputeTotalValue();

            Assert.True(reaults == sum);
        }

        [Theory]
        [InlineData(0, 1, 0, 1)]
        public void Clear_ValidArgumetn_ClearContext(int firstIndex, int secondIndex, int lenght, int quantity)
        {
            var products = GetProducts();
            var p1 = products.ElementAt(firstIndex);
            var p2 = products.ElementAt(secondIndex);
            var target = GetCart();

            target.AddItem(p1, quantity);
            target.AddItem(p2, quantity);

            target.Clear();
            var reaults = target.Lines.Count();
                

            Assert.True(reaults == lenght);
        }

        private IQueryable<Product> GetProducts() => GetterFakeRepository.GetProductWithCategory();

        private Cart GetCart() => new Cart();
    }
}
