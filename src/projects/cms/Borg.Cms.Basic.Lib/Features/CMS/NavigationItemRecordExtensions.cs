using Borg.Infra.DTO;
using Borg.Platform.EF.CMS;
using System.Collections.Generic;
using System.Linq;

namespace Borg
{
    public static class NavigationItemRecordExtensions
    {
        public static Tidings Trees(this IEnumerable<NavigationItemState> source)
        {
            var roots = source.Where(x => x.Taxonomy.ParentId == 0 || !source.Any(r => x.Taxonomy.ParentId == r.Id));
            var result = new Tidings();
            foreach (var root in roots)
            {
                result.Add(ToTiding(root, source));
            }
            return result;
        }

        public static Tidings Trees(this IEnumerable<NavigationItemState> source, int rootId)
        {
            var roots = source.Where(x => x.Id == rootId);
            var result = new Tidings();
            foreach (var root in roots.OrderBy(x => x.Taxonomy.Weight).ThenBy(x => x.Article.Title))
            {
                result.Add(ToTiding(root, source));
            }
            return result;
        }

        public static IDictionary<(int, int), Tiding> TreeDictionaryExcludingCurrentAndChildren(this IEnumerable<NavigationItemState> source, Tiding current)
        {
            var navigationItemRecords = source as NavigationItemState[] ?? source.ToArray();
            var entity = navigationItemRecords.FirstOrDefault(x => x.Id.ToString() == current?.Key);
            return navigationItemRecords.TreeDictionaryExcludingCurrentAndChildren(entity);
        }

        public static IDictionary<(int, int), Tiding> TreeDictionaryExcludingCurrentAndChildren(this IEnumerable<NavigationItemState> source, NavigationItemState current)
        {
            var navigationItemRecords = source.ToList();
            var tree = navigationItemRecords.Trees().TreeDictionary();
            if (current == null) return tree;
            var hit = tree.FirstOrDefault(x => x.Key.Item1 == current?.Id);
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

        private static Tiding ToTiding(NavigationItemState state, IEnumerable<NavigationItemState> source, int level = 1)
        {
            var path = state.Path;
            if (level > 1)
            {
                var pid = state.Taxonomy.ParentId;
                var parent = source.Single(x => x.Id == pid);
                path = parent.Path.Trim('/') + "/" + path.Trim('/');
            }
            var tiding = new Tiding(state.Id.ToString(), state.Display)
            {
                Weight = state.Taxonomy.Weight,
                Flag = (state.Component.IsPublished && !state.Component.IsDeleted).ToString(),
                Hint = state.NavigationItemType.ToString(),
                HumanKey = path,
                Depth = level
            };
            foreach (var row in source.Where(x => x.Taxonomy.ParentId == state.Id).OrderBy(x => x.Taxonomy.Weight).ThenBy(x => x.Article.Title))
            {
                tiding.Children.Add(ToTiding(row, source, level + 1));
            }
            return tiding;
        }
    }
}