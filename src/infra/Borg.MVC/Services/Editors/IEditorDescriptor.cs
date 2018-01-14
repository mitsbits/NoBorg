namespace Borg.MVC.Services.Editors
{
    public interface IEditorDescriptor
    {
        string FriendlyName { get; }
        string EditorType { get; }
    }
}