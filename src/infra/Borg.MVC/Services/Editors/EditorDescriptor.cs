namespace Borg.MVC.Services.Editors
{
    public abstract class EditorDescriptor : IEditorDescriptor
    {
        public abstract string FriendlyName { get; }
        public abstract string EditorType { get; }
    }
}