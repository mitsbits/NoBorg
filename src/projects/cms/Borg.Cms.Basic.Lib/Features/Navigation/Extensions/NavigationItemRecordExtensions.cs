using Borg.Infra.DTO;
using System.Collections.Generic;
using System.Linq;
using Borg.Cms.Basic.Lib.Features.Navigation;

namespace Borg
{
    internal static class NavigationItemRecordExtensions
    {
        public static Tidings Trees(this IEnumerable<NavigationItemRecord> source)
        {
            var roots = source.Where(x => x.ParentId == 0 || !source.Any(r => x.ParentId == r.Id));
            var result = new Tidings();
            foreach (var root in roots)
            {
                result.Add(ToTiding(root, source));
            }
            return result;
        }

        public static Tidings Trees(this IEnumerable<NavigationItemRecord> source, int rootId)
        {
            var roots = source.Where(x => x.Id == rootId);
            var result = new Tidings();
            foreach (var root in roots.OrderBy(x=>x.Weight).ThenBy(x=>x.Display))
            {
                result.Add(ToTiding(root, source));
            }
            return result;
        }

        public static IDictionary<(int, int), Tiding> TreeDictionaryExcludingCurrentAndChildren(this IEnumerable<NavigationItemRecord> source, Tiding current)
        {
            var navigationItemRecords = source as NavigationItemRecord[] ?? source.ToArray();
            var entity = navigationItemRecords.FirstOrDefault(x => x.Id.ToString() == current?.Key);
            return navigationItemRecords.TreeDictionaryExcludingCurrentAndChildren(entity);
        }

        public static IDictionary<(int, int), Tiding> TreeDictionaryExcludingCurrentAndChildren(this IEnumerable<NavigationItemRecord> source, NavigationItemRecord current)
        {
            var navigationItemRecords = source.ToList();
            var tree = navigationItemRecords.Trees().TreeDictionary();
            var hit = tree.FirstOrDefault(x => x.Key.Item1 == current.Id);
            if (hit.Key.Item1 == 0 && hit.Key.Item2 == 0) return tree;

            var keystoexclude = new List<(int, int)>();
            RecurseToExclude(ref keystoexclude, current.Id, tree);
            foreach (var exclude in keystoexclude)
            {
                tree.Remove(exclude);
            }
            return tree;
        }

        private static void RecurseToExclude(ref List<(int, int)> keystoexclude, int id, IDictionary<(int, int), Tiding> source)
        {
            var current = source.FirstOrDefault(x => x.Key.Item1 == id);
            if (current.Key.Item1 == 0 && current.Key.Item2 == 0) return;
            keystoexclude.Add(current.Key);
            foreach (var child in current.Value.Children)
            {
                RecurseToExclude(ref keystoexclude, int.Parse(child.Key), source);
            }
        }

        private static Tiding ToTiding(NavigationItemRecord record, IEnumerable<NavigationItemRecord> source, int level = 1)
        {
            var tiding = new Tiding(record.Id.ToString(), record.Display)
            {
                Weight = record.Weight,
                Flag = record.IsPublished.ToString(),
                Hint = record.ItemType.ToString(),
                HumanKey = record.Path,
                Depth = level
            };
            foreach (var row in source.Where(x => x.ParentId == record.Id).OrderBy(x => x.Weight).ThenBy(x => x.Display))
            {
                tiding.Children.Add(ToTiding(row, source, level + 1));
            }
            return tiding;
        }
    }
}