using System;

namespace Borg.CMS.BackOfficeInstructions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EditorTabAttribute : Attribute
    {
        public EditorTabAttribute(string tab)
        {
            Tab = tab;
        }

        public string Tab { get; }
    }
}