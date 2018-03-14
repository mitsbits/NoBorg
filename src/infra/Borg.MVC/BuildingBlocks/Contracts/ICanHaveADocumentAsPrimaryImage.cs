namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface ICanHaveADocumentAsPrimaryImage
    {
        string PrimaryImageFileId { get; set; }
    }
}