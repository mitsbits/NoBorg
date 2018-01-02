using Borg.Infra.Collections.Hierarchy;
using Borg.Infra.DDD;
using Borg.Infra.DTO;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Borg.Cms.Basic.Lib.Features.Navigation
{
    public class NavigationItemRecord : IHasParent<int>, IPublishable, IEntity<int>, IWeighted
    {
        public int Id { get; set; }
        public int Depth { get; protected set; }

        [DefaultValue(0)]
        [DisplayName("Parent")]
        [Required]
        public int ParentId { get; set; }

        [DisplayName("Display")]
        [Required]
        public string Display { get; set; }

        [DefaultValue("BSE")]
        [DisplayName("Group")]
        [Required]
        public string Group { get; set; }

        [DefaultValue("/")]
        [DisplayName("Path")]
        public string Path { get; set; }

        [DisplayName("Item Type")]
        [Required]
        public NavigationItemType ItemType { get; set; } = NavigationItemType.Label;

        [DefaultValue(true)]
        [DisplayName("Active")]
        [Required]
        public bool IsPublished { get; set; }

        public void Publish()
        {
            IsPublished = true;
        }

        public void Suspend()
        {
            IsPublished = false;
        }

        [DefaultValue(0)]
        [DisplayName("Weight")]
        public double Weight { get; protected set; }
    }

    public static class NavigationItemRecordExtensions
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
            foreach (var root in roots)
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
            foreach (var row in source.Where(x => x.ParentId == record.Id))
            {
                tiding.Children.Add(ToTiding(row, source, level + 1));
            }
            return tiding;
        }
    }
}