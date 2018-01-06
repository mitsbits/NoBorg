using System.Collections.Generic;
using System.Linq;

namespace Borg.Infra.DTO
{
    public static class TidingExtensions
    {
        public static IEnumerable<Tiding> Flatten(this Tidings tidings)
        {
            var result = new List<Tiding>();
            foreach (var tiding in tidings)
                RecurseAdd(tiding, ref result);
            return result;
        }

        private static void RecurseAdd(Tiding tiding, ref List<Tiding> output)
        {
            output.Add(tiding);
            foreach (var child in tiding.Children)
                RecurseAdd(child, ref output);
        }

        public static void SetValue<T>(this Tiding tiding, T value, ISerializer serializer = null)
        {
            serializer = serializer ?? new JsonNetSerializer();
            tiding.Value = AsyncHelpers.RunSync(() => serializer.SerializeToStringAsync(value));
            tiding.Hint = typeof(T).FullName;
        }

        public static T GetValue<T>(this Tiding tiding, ISerializer serializer = null)
        {
            serializer = serializer ?? new JsonNetSerializer();
            return AsyncHelpers.RunSync(() => serializer.DeserializeAsync<T>(tiding.Value));
        }
    }

    public static class TidingsExtensions
    {
        public static IDictionary<(int, int), Tiding> TreeDictionary(this Tidings tidings)
        {
            var bucket = new Dictionary<(int, int), Tiding>();
            foreach (var tiding in tidings.AsEnumerable().OrderBy(x => x.Weight).ThenBy(x => x.Value))
            {
                int i = 1;
                RecurseTididnigsToDictionary(tiding, bucket, i);
            }
            return bucket;
        }

        private static void RecurseTididnigsToDictionary(Tiding tiding, Dictionary<(int, int), Tiding> bucket, int i)
        {
            var key = (int.Parse(tiding.Key), i);
            bucket.Add(key, tiding);
            i++;
            foreach (var child in tiding.Children.AsEnumerable().OrderBy(x => x.Weight).ThenBy(x => x.Value))
            {
                RecurseTididnigsToDictionary(child, bucket, i);
            }
        }
    }
}