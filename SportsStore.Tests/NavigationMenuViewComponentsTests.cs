using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using SportsStore.Components;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentsTests
    {
        [Fact]
        public void Invoke_ValidArgument_CanSelectCategories()
        {
            var target = GetNavigationMenuComponents();
            var categories = GetProducts().Select(x => x.Category).Distinct().ToArray();

            var result =((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

            Assert.True(Enumerable.SequenceEqual(categories, result));
        }

        [Theory]
        [InlineData("Cat1")]
        [InlineData("Cat2")]
        [InlineData("Cat3")]
        public void Invoke_ValidArgument_IndicatesSelectedCategory(string categoryName)
        {
            var target = GetNavigationMenuComponents();
            target.ViewComponentContext = GetViewComponentContext();
            target.RouteData.Values["category"] = categoryName;

            var result =(string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            Assert.Equal(categoryName, result);
        }


        private ViewComponentContext GetViewComponentContext() => new ViewComponentContext
        {
            ViewContext = new ViewContext
            {
                RouteData = new RouteData()
            }
        };

        private IQueryable<Product> GetProducts() => GetterFakeRepository.GetProductWithCategory();

        private IProductRepository GetProductRepository() => GetterFakeRepository.GetProductRepository(GetterFakeRepository.GetProductWithCategory());
        private NavigationMenuViewComponent GetNavigationMenuComponents() => new NavigationMenuViewComponent(GetProductRepository());
    }
}
