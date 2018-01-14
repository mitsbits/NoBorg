using System.Collections.Generic;

namespace Borg.MVC.Services.Editors
{
    public class EditorGroup
    {
        public EditorGroup()
        {
        }

        public EditorGroup(string identifier)
        {
            GroupIdentifier = identifier;
        }

        public string GroupIdentifier { get; set; }
        public List<Editor> Editors { get; set; } = new List<Editor>();
    }

    public class EditorForm
    {
        public List<EditorGroup> EditorGroups { get; set; } = new List<EditorGroup>();
    }

    public interface IEditorsProvider
    {
        IEnumerable<Editor> Editors();
    }
}