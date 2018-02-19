using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Infra.DTO
{
    public class Tidings : ICollection<Tiding>, IDictionary<string, string>, IReadOnlyList<Tiding>, IEnumerable<Tiding>, ICloneable, ICloneable<Tidings>
    {
        private readonly ICollection<Tiding> _bucket;

        public Tidings()
        {
            _bucket = new HashSet<Tiding>();
        }

        public Tidings(IEnumerable<Tiding> source)
        {
            _bucket = new HashSet<Tiding>(source);
        }

        public IReadOnlyDictionary<string, Tidings> RootsByKey
        {
            get { return _bucket.GroupBy(x => x.Key).ToDictionary(x => x.Key, x => new Tidings(x.Select(y => y))); }
        }

        public IReadOnlyDictionary<string, Tiding> RootByKey
        {
            get { return _bucket.GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.First()); }
        }

        public IEnumerable<Tiding> AsEnumerable()
        {
            return this;
        }

        #region IReadOnlyCollection

        public Tiding this[int index] => _bucket.ToArray()[index];

        #endregion IReadOnlyCollection

        public static class DefinedKeys
        {
            public const string Id = "Id";
            public const string Key = "Key";
            public const string Group = "Group";
            public const string State = "State";
            public const string Identifier = "Identifier";
            public const string Name = "Name";
            public const string Display = "Display";
            public const string Payload = "Payload";
            public const string View = "View";
            public const string AssemblyQualifiedName = "AssemblyQualifiedName";
        }

        #region ICollection

        public void Add(Tiding item)
        {
            Preconditions.NotNull(item, nameof(item));
            _bucket.Add(item);
        }

        public void Clear()
        {
            _bucket.Clear();
        }

        public bool Contains(Tiding item)
        {
            Preconditions.NotNull(item, nameof(item));
            return _bucket.Contains(item);
        }

        public void CopyTo(Tiding[] array, int arrayIndex)
        {
            _bucket.CopyTo(array, arrayIndex);
        }

        public int Count => _bucket.Count;

        public bool IsReadOnly => _bucket.IsReadOnly;

        public bool Remove(Tiding item)
        {
            if (!_bucket.Contains(item)) return false;
            _bucket.Remove(item);
            return true;
        }

        public IEnumerator<Tiding> GetEnumerator()
        {
            return _bucket.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _bucket.GetEnumerator();
        }

        #endregion ICollection

        #region IDictionary

        public void Add(string key, string value)
        {
            if (_bucket.Any(x => x.Key == key))
                _bucket.Single(x => x.Key == key).Value = value;
            else
                _bucket.Add(new Tiding(key, value));
        }

        public bool ContainsKey(string key)
        {
            return _bucket.Any(x => x.Key == key);
        }

        public ICollection<string> Keys
        {
            get { return _bucket.Select(x => x.Key).Distinct().ToList(); }
        }

        public bool Remove(string key)
        {
            var hit = _bucket.FirstOrDefault(x => x.Key == key);
            return hit != null && _bucket.Remove(hit);
        }

        public bool TryGetValue(string key, out string value)
        {
            var hit = _bucket.FirstOrDefault(x => x.Key == key);
            if (hit == null)
            {
                value = null;
                return false;
            }
            value = hit.Value;
            return true;
        }

        public ICollection<string> Values
        {
            get { return _bucket.Select(x => x.Value).ToList(); }
        }

        public string this[string key]
        {
            get
            {
                var hit = _bucket.FirstOrDefault(x => x.Key == key);
                return hit != null ? hit.Value : string.Empty;
            }
            set
            {
                var hit = _bucket.FirstOrDefault(x => x.Key == key);
                if (hit != null)
                    hit.Value = value;
                else
                    _bucket.Add(new Tiding(key, value));
            }
        }

        public void Add(KeyValuePair<string, string> item)
        {
            if (_bucket.Any(x => x.Key == item.Key))
                _bucket.Single(x => x.Key == item.Key).Value = item.Value;
            else
                _bucket.Add(new Tiding(item.Key, item.Value));
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return _bucket.Any(x => x.Key == item.Key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            var dic = from t in _bucket select new KeyValuePair<string, string>(t.Key, t.Value);
            dic.ToArray().CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            var hit = _bucket.FirstOrDefault(x => x.Key == item.Key);
            if (hit == null) return false;
            return _bucket.Remove(hit);
        }

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        {
            var dic = from t in _bucket select new KeyValuePair<string, string>(t.Key, t.Value);
            return dic.GetEnumerator();
        }

        #endregion IDictionary

        #region ICloneable

        object ICloneable.Clone()
        {
            return Clone();
        }

        public Tidings Clone()
        {
            var children = this.Flatten().Select(x => x.Clone()).ToList();
            var clone = new Tidings(children);
            return clone;
        }

        #endregion ICloneable
    }
}