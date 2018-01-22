namespace Borg.CMS.Components.Contracts
{
    public interface ICanBeDeleted
    {
        void Delete();

        bool IsDeleted { get; }
    }
}