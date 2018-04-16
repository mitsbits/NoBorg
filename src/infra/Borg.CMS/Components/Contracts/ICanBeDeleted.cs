namespace Borg.CMS.Components.Contracts
{
    public interface ICanBeDeleted
    {
        void Delete();

        bool IsDeleted { get; }
    }

    public interface ICanDeletedAndRecovered: ICanBeDeleted
    {
        void Recover();


    }
}