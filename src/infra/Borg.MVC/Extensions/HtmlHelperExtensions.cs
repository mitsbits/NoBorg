using Borg.CMS.BuildingBlocks.Contracts;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Borg
{
    public static partial class HtmlHelperExtensions
    {
        public static async Task<IHtmlContent> RenderViewSection(this IHtmlHelper html, IViewComponentHelper componentHelper, ISection section, Action<Exception> handleException = null)
        {
            IEnumerable<ISlot> items =
                section.Slots.Where(x => x.SectionSlotInfo.Enabled)
                    .OrderBy(x => x.SectionSlotInfo.Ordinal)
                    .ToList();
            if (!items.Any()) return HtmlString.Empty;
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                using (var writer = new IndentedTextWriter(new StringWriter(sb)))
                {
                    try
                    {
                        if (item.Module.ModuleGender == ModuleGender.ViewComponent.Flavor)
                        {
                            var tx = item.Module.Parameters.FirstOrDefault(x => x.Key == "AssemblyQualifiedName").Value;
                            var type = Type.GetType(tx);
                            var result = await componentHelper.InvokeAsync(type, new Tidings(item.Module.Parameters));
                            result.WriteTo(writer, HtmlEncoder.Default);
                        }

                        if (item.Module.ModuleGender == ModuleGender.PartialView.Flavor)
                        {
                            var view = item.Module.Parameters.FirstOrDefault(x => x.Key == Tidings.DefinedKeys.View).Value;
                            var result = await html.PartialAsync(view, new Tidings(item.Module.Parameters));
                            result.WriteTo(writer, HtmlEncoder.Default);
                        }

                        //var toRender = item.Module as IHtmlHelperRendersMe;
                        //toRender?.Render(html);
                    }
                    catch (Exception ex)
                    {
                        handleException?.Invoke(ex);
                    }
                }
            }
            return html.Raw(sb.ToString());
        }
    }
}