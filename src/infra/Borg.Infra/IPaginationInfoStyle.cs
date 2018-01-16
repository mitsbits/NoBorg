namespace Borg.Infra
{
    public interface IPaginationInfoStyle
    {
        string ItemSliceAndTotalFormat { get; }
        string PageCountAndLocationFormat { get; }
        string NextDisplay { get; }
        string LastDisplay { get; }
        string PreviousDisplay { get; }
        string FirstDisplay { get; }
        string PageDisplayFormat { get; }
        string PageVariableName { get; }
        string ElementClass { get; }
        string CurrentClass { get; }
        string UnavailableClass { get; }
        string ArrowClass { get; }
        string Ellipses { get; }
        string OutputTagElement { get; }

        string OutputItemTagElement { get; }
    }
}