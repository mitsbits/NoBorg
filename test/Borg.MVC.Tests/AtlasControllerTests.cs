using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Xunit;

namespace Borg.MVC.Tests
{
    public class BorgControllerTests
    {
        private readonly ViewContext _vc;
        private readonly ControllerContext _context;
        private ConcreteController _ctrl;

        [Fact]
        public void controller_should_have_default_page_content()
        {
            _ctrl = new ConcreteController(new NullLoggerFactory());
            var page = _ctrl.PageContentTest<PageContent>();
            page.ShouldNotBeNull();
        }

        [Fact]
        public void controller_should_be_able_to_change_the_page_title_and_subtitle()
        {
            _ctrl = new ConcreteController(new NullLoggerFactory());
            _ctrl.SetPageTitleTest("new title", "some subtitle");
            var page = _ctrl.PageContentTest<PageContent>();
            page.ShouldNotBeNull();
            page.Title.ShouldBe("new title", Case.Insensitive);
            page.Subtitle.ShouldBe("some subtitle", Case.Insensitive);
        }

        [Fact]
        public void controller_should_have_default_device()
        {
            _ctrl = new ConcreteController(new NullLoggerFactory());
            var device = _ctrl.GetDevice<Device>();
            device.ShouldNotBeNull();
        }
    }

    internal class ConcreteController : BorgController
    {
        internal void SetPageTitleTest(string title, string subtitle = "")
        {
            base.SetPageTitle(title, subtitle);
        }

        internal TContent PageContentTest<TContent>(TContent content = default(TContent)) where TContent : IPageContent
        {
            return PageContent(content);
        }

        public ConcreteController(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}