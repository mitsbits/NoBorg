using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Borg.MVC.Services
{
    public class DeviceLayoutFileProvider : IDeviceLayoutFileProvider
    {
        private const string _defaultTemplateFolder = "Templates";
        private readonly string[] _sectionTriggers = { @"RenderSectionAsync\(", @"RenderSection\(" }; //the order is important
        private readonly BorgSettings _settings;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly List<DeviceLayoutFileInfo> _layouts = new List<DeviceLayoutFileInfo>();
        private bool _hasInitialised = false;

        public DeviceLayoutFileProvider(IHostingEnvironment hostingEnvironment, BorgSettings settings)
        {
            _settings = settings;
            _hostingEnvironment = hostingEnvironment;
        }

        private void Init()
        {
            _layouts.Clear();
            var root = _hostingEnvironment.ContentRootPath;
            var defaultTemplatesPath = Path.Combine(root, "Views", "Shared", _defaultTemplateFolder);
            var sharedTemplatesPath = Path.Combine(root, "Views", "Shared");

            var files1 = Directory.GetFiles(defaultTemplatesPath, "*.cshtml", SearchOption.TopDirectoryOnly);
            var files2 = Directory.GetFiles(sharedTemplatesPath, "*.cshtml", SearchOption.TopDirectoryOnly);

            var hits = files1.Union(files2).Distinct();
            var layoutsections = new List<string>();
            foreach (var hit in hits)
            {
                var matches = new List<Match>();
                var txt = File.ReadAllText(hit);
                foreach (var sectionTrigger in _sectionTriggers)
                {
                    var match = Regex.Matches(txt, sectionTrigger);
                    Match[] col = new Match[match.Count];
                    match.CopyTo(col, 0);
                    matches.AddRange(col);

                    foreach (var candidate in matches)
                    {
                        var temp = txt.Substring(candidate.Index + candidate.Length);
                        var startindx = temp.IndexOf('"') + 1;

                        var c = temp[startindx];
                        var sb = new StringBuilder(c.ToString());
                        while (c != '"')
                        {
                            startindx++;
                            c = temp[startindx];
                            sb.Append(c);
                        }
                        layoutsections.Add(sb.ToString().TrimEnd('"'));
                    }
                }

                if (layoutsections.Any())
                {
                    var dl = new DeviceLayoutFileInfo()
                    {
                        FullPath = $"~/Views/{hit.Substring(hit.IndexOf("Views/") + 5)}".Replace(@"\", "/"),
                        SectionIdentifiers = layoutsections.Distinct().ToArray()
                    };
                    _layouts.Add(dl);
                }
            }
            _hasInitialised = true;
        }

        public Task<IEnumerable<IDeviceLayoutFileInfo>> LayoutFiles()
        {
            if (!_hasInitialised) Init();
            return Task.FromResult(_layouts.Cast<IDeviceLayoutFileInfo>());
        }

        public void Invalidate()
        {
            _hasInitialised = false;
        }
    }
}