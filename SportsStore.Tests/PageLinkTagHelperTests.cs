using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SportsStore.Tests
{
    public class PageLinkTagHelperTests
    {

        [Fact]
        public void Process_ValidArgument_RightContent()
        {
            var output = GetTagHelperOutput();
            var ctx = GetTagHelperContext();
            var helper = GetPageLinkTagHelper();

            helper.Process(ctx, output);

            Assert.Equal(
                @"<a href=""Test/Page1"">1</a>"
                +  
                @"<a href=""Test/Page2"">2</a>"
                + 
                @"<a href=""Test/Page3"">3</a>",
                output.Content.GetContent()
                );
        }

        private IUrlHelper GetUrlHelper()
        {
            var helper = new Mock<IUrlHelper>();
            helper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");

            return helper.Object;
        }

        private IUrlHelperFactory GetUrlHelperFactory()
        {
            var urlFactory = new Mock<IUrlHelperFactory>();
            urlFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(GetUrlHelper());

            return urlFactory.Object;
        }

        private TagHelperContent GetTagHelperContent() => new Mock<TagHelperContent>().Object;

        private PageLinkTagHelper GetPageLinkTagHelper() => new PageLinkTagHelper(GetUrlHelperFactory())
        {
            PageModel = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            },
            PageAction = "Test"
        };

        private TagHelperContext GetTagHelperContext() => new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "");

        private TagHelperOutput GetTagHelperOutput() => new TagHelperOutput(
            "div",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult(GetTagHelperContent())
            );
    }
}
