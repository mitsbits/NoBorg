namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IHaveAUrl
    {
        string Path { get; }
        string QueryString { get; }
        string Domain { get; }
    }
}