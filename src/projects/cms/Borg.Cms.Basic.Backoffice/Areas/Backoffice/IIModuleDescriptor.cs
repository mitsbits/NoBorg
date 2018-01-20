namespace Borg.Cms.Basic.Backoffice.Areas.Backoffice
{
    interface IIModuleDescriptor
    {
        string Area { get; }
        string[] ModuleThemes { get; }
    }
}