using Borg.Infra;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.PlugIns.Contracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;

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
        private readonly IPlugInHost _plugInHost;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly Assembly[] _entrypointassemblies;
        private readonly List<EmbeddedFileProvider> _fileProviders = new List<EmbeddedFileProvider>();

        public DeviceLayoutFileProvider(IHostingEnvironment hostingEnvironment, BorgSettings settings, IPlugInHost plugInHost, IRazorViewEngine razorViewEngine, Assembly[] entrypointassemblies)
        {
            _settings = settings;
            _plugInHost = plugInHost;
            _razorViewEngine = razorViewEngine;
            _entrypointassemblies = entrypointassemblies;
            _hostingEnvironment = hostingEnvironment;

            foreach (var entrypointassembly in entrypointassemblies)
            {
                _fileProviders.Add(new EmbeddedFileProvider(entrypointassembly));
            }

        }

        private List<IFileInfo> _enmbeddedPaths = new List<IFileInfo>();
        private void Init()
        {
            _layouts.Clear();

            foreach (var f in _fileProviders)
            {
                _enmbeddedPaths.AddRange(f.GetDirectoryContents(""));
            }

            var containers = _enmbeddedPaths.Where(x => x.Name.Contains(".Shared.")).ToList();
            foreach (var container in containers)
            {
                using (var stream = container.CreateReadStream())
                {
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        var str = Encoding.UTF8.GetString(ms.ToArray());
                        var layoutsections = new List<string>();

                        var txt = str;
                        foreach (var sectionTrigger in _sectionTriggers)
                        {
                            var match = Regex.Matches(txt, sectionTrigger);
                            var matcharray = new Match[match.Count];
                            var matches = new List<Match>();
                            match.CopyTo(matcharray, 0);
                            matches.AddRange(matcharray);

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
                                SectionIdentifiers = layoutsections.Distinct().ToArray()
                            };
                            var local = container.Name.Substring(container.Name.IndexOf("Themes\\", StringComparison.Ordinal) + 5);
                            if (container.Name.Contains("Themes."))
                            {
                                var left = container.Name.Substring(container.Name.IndexOf("Themes.") + 7);
                                dl.Theme = left.Substring(0, left.IndexOf('.'));
                            }
                            else
                            {
                                dl.Theme = "";
                            }
               
                            dl.FullPath = container.Name;
                            _layouts.Add(dl);
                        }
                    }
                }

            }




            var root = _hostingEnvironment.ContentRootPath;
            var defaultTemplatesPath = Path.Combine(root, "Views", "Shared", _defaultTemplateFolder);
            var sharedTemplatesPath = Path.Combine(root, "Views", "Shared");
            //TODO: add from settings

            var files1 = Directory.Exists(defaultTemplatesPath)
                ? Directory.GetFiles(defaultTemplatesPath, "*.cshtml", SearchOption.TopDirectoryOnly)
                : new string[0];
            var files2 = Directory.Exists(sharedTemplatesPath) ? Directory.GetFiles(sharedTemplatesPath, "*.cshtml", SearchOption.TopDirectoryOnly) : new string[0];

            var hits = files1.Union(files2).Distinct();

            var themeFiles = new List<string>();

            var themedPlugIns = _plugInHost.SpecifyPlugins<IPlugInTheme>();

            foreach (var themedPlugIn in themedPlugIns)
            {
                foreach (var theme in themedPlugIn.Themes)
                {
                    var defaultThemeTemplatesPath = Path.Combine(root, "Themes", theme, "Shared", _defaultTemplateFolder);
                    var sharedThemeTemplatesPath = Path.Combine(root, "Themes", theme, "Shared");
                    var tfiles1 = Directory.Exists(defaultThemeTemplatesPath) ? Directory.GetFiles(defaultThemeTemplatesPath, "*.cshtml", SearchOption.TopDirectoryOnly) : new string[0];
                    var tfiles2 = Directory.Exists(sharedThemeTemplatesPath) ? Directory.GetFiles(sharedThemeTemplatesPath, "*.cshtml", SearchOption.TopDirectoryOnly) : new string[0];
                    var thits = tfiles1.Union(tfiles2).Distinct();
                    foreach (var hit in thits)
                    {
                        var layoutsections = new List<string>();

                        var txt = File.ReadAllText(hit);
                        foreach (var sectionTrigger in _sectionTriggers)
                        {
                            var match = Regex.Matches(txt, sectionTrigger);
                            var matcharray = new Match[match.Count];
                            var matches = new List<Match>();
                            match.CopyTo(matcharray, 0);
                            matches.AddRange(matcharray);

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
                                SectionIdentifiers = layoutsections.Distinct().ToArray()
                            };
                            var local = hit.Substring(hit.IndexOf("Themes\\", StringComparison.Ordinal) + 5);
                            dl.Theme = theme;
                            dl.FullPath = $"~/Themes/{local}".Replace(@"\", "/");
                            _layouts.Add(dl);
                        }
                    }
                }
            }

            foreach (var hit in hits)
            {
                var layoutsections = new List<string>();

                var txt = File.ReadAllText(hit);
                foreach (var sectionTrigger in _sectionTriggers)
                {
                    var match = Regex.Matches(txt, sectionTrigger);
                    var matcharray = new Match[match.Count];
                    var matches = new List<Match>();
                    match.CopyTo(matcharray, 0);
                    matches.AddRange(matcharray);

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
                        SectionIdentifiers = layoutsections.Distinct().ToArray()
                    };
                    var local = hit.Substring(hit.IndexOf("Views\\", StringComparison.Ordinal) + 5);
                    dl.FullPath = $"~/Views/{local}".Replace(@"\", "/");
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