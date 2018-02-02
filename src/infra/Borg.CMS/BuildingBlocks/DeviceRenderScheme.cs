namespace Borg.CMS.BuildingBlocks
{
    public static class DeviceRenderScheme
    {
        public const string UnSet = "UnSet";
        public const string Detail = "Detail";
        public const string List = "List";

        public static string[] Schemes()
        {
            return new[] { UnSet, Detail, List };
        }
    }
}