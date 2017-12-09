using Borg.Infra.Collections.Hierarchy;

namespace Borg.Infra.DTO
{
    public class Steps : Tidings, IHierarchicalEnumerable
    {
        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return enumeratedItem as IHierarchyData;
        }
    }
}