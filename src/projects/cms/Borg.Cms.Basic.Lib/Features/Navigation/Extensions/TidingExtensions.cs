using Borg.Infra.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Borg
{
    public static class TidingExtensions
    {
        public static string ToBootstrapTree(this Tidings tidings, IUrlHelper url, string @group, int? row)
        {
            var bucket = new List<dynamic>();
            foreach (var root in tidings.AsEnumerable().OrderBy(x => x.Weight).ThenBy(x => x.Value))
            {
                bucket.Add(PopulateNode(root, url, @group, row));
            }
            var toserialize = bucket.ToArray();
            return JsonConvert.SerializeObject(toserialize);
        }

        private static dynamic PopulateNode(Tiding root, IUrlHelper url, string @group, int? row, bool parentEnabled = true)
        {
            dynamic node = new ExpandoObject();
            node.text = root.Value;
            node.href = url.Action("Home", "Menus", new { id = @group, row = root.Key });
            node.selectable = false;
            var isEnabled = bool.Parse(root.Flag);
            dynamic state = new { @checked = isEnabled, selected = row.HasValue && row.Value.ToString() == root.Key };
            var tags = new[] { root.Hint, root.Weight.ToString("n2") };
            node.tags = tags;
            node.state = state;
            if (!parentEnabled || !isEnabled) node.color = "#a94442";

            var children = new List<dynamic>();
            foreach (var child in root.Children.AsEnumerable().OrderBy(x => x.Weight).ThenBy(x => x.Value))
            {
                children.Add(PopulateNode(child, url, @group, row, isEnabled));
            }
            if (children.Any()) node.nodes = children.ToArray();
            return node;
        }
    }
}