using System;

namespace Borg.MVC.PlugIns.Decoration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PulgInTagHelperAttribute : Attribute
    {
        public PulgInTagHelperAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}