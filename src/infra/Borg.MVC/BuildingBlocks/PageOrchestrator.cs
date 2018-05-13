using Borg.Infra;
using Borg.MVC.BuildingBlocks.Contracts;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace Borg.MVC.BuildingBlocks
{
    public class PageOrchestrator : IPageContentAccessor<IPageContent>,
        IDeviceAccessor<IDevice>,
        IViewContextAware,
        IPageOrchestrator<IPageContent, IDevice>,
        ICanContextualizeFromController,
        ICanContextualizeFromView
    {
        private IPageContent _page;
        private readonly Func<IPageContent> _defaultContent = () => new PageContent() { Title = "Default" };

        private IDevice _device;
        private readonly Func<IDevice> _defaultDevice = () => new Device() { FriendlyName = "Default", Path = string.Empty, Layout = string.Empty };

        public void Contextualize(ViewContext viewContext)
        {
            if (ContextAcquired || Bag != null) return;
            Preconditions.NotNull(viewContext, nameof(viewContext));
            Bag = viewContext.ViewBag;
            Device.TryContextualize(viewContext);
            ContextAcquired = true;
        }

        public void Contextualize(Controller controller)
        {
            if (ContextAcquired || Bag != null) return;
            Preconditions.NotNull(controller, nameof(controller));
            Bag = controller.ViewBag;
            Device.TryContextualize(controller);
            ContextAcquired = true;
        }

        public IPageContent Page => GetPage();

        private IPageContent GetPage()
        {
            var page = _page ?? (_page = Bag.ContentInfo as IPageContent);
            if (page != null) return _page;
            _page = _defaultContent.Invoke();
            Bag.ContentInfo = _page;
            return _page;
        }

        public IDevice Device => GetDevice();

        private IDevice GetDevice()
        {
            var device = _device ?? (_device = Bag.DeviceInfo as IDevice);
            if (device != null) return _device;
            _device = _defaultDevice.Invoke();
            Bag.DeviceInfo = _device;
            return _device;
        }

        private dynamic Bag { get; set; }
        public bool ContextAcquired { get; private set; } = false;
    }
}