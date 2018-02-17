using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Borg.Infra.Storage.Assets
{
    public class VisualSize
    {
        private readonly string _flavorDescription;
        private static readonly ConcurrentDictionary<string, VisualSize> Dict = new ConcurrentDictionary<string, VisualSize>();

        public static readonly VisualSize Undefined = new VisualSize(nameof(Undefined));
        public static readonly VisualSize Tiny = new VisualSize(nameof(Tiny));
        public static readonly VisualSize Small = new VisualSize(nameof(Small));
        public static readonly VisualSize Medium = new VisualSize(nameof(Medium));
        public static readonly VisualSize Large = new VisualSize(nameof(Large));
        public static readonly VisualSize Huge = new VisualSize(nameof(Huge));

        public VisualSize()
        {

        }
        private VisualSize(string flavorDescription)
        {
            Preconditions.NotEmpty(flavorDescription, nameof(flavorDescription));
            _flavorDescription = flavorDescription;
            if (!Dict.ContainsKey(_flavorDescription))
                Dict.TryAdd(flavorDescription, this);
        }

        public override string ToString()
        {
            return _flavorDescription;
        }

        public string Flavor => ToString();

        public static VisualSize Parse(string flavorDescription)
        {
            return Dict.Keys.Contains(flavorDescription) ? Dict[flavorDescription] : Undefined;
        }

        public static bool TryParse(string flavorDescription, out VisualSize size)
        {
            try
            {
                size = Parse(flavorDescription);
                return true;
            }
            catch (Exception ex)
            {
                size = Undefined;
                return false;
            }
        }

        public static List<VisualSize> GetMembers()
        {
            return new List<VisualSize>(Dict.Values);
        }
    }
}