using System;
using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg.MVC.BuildingBlocks
{
    public class DeviceLayoutFileInfo : IDeviceLayoutFileInfo
    {
        public string Theme { get; set; }
        public string FullPath { get; set; }
        public string[] SectionIdentifiers { get; set; }
        public bool MatchesPath(string absolutePath)
        {
            if (string.Equals(absolutePath, FullPath, StringComparison.CurrentCultureIgnoreCase)) return true;
            if (System.IO.Path.IsPathRooted(absolutePath))
            {
                var local = absolutePath.Substring(absolutePath.IndexOf("Views\\", StringComparison.Ordinal) + 5);
                var global = $"~/Views/{local}".Replace(@"\", "/");
                return string.Equals(global, FullPath, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;

        }
    }
}