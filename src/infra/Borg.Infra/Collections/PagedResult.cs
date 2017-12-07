using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Infra.Collections
{
    public class PagedResult<T> : IPagedResult<T>
    {
        #region Declarations

        private readonly List<T> _data = new List<T>();

        #endregion Declarations

        #region Public Constructor

        public PagedResult(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords) : this()
        {
            Clear();
            AddRange(data);
            TotalRecords = totalRecords;
            PageSize = pageSize;
            Page = Math.Min(Math.Max(1, pageNumber), TotalPages);
        }

        public PagedResult()
        {
            Clear();

            TotalRecords = 0;
            PageSize = 10;
            Page = Math.Min(Math.Max(1, PageSize), TotalPages);
        }

        #endregion Public Constructor

        #region IPagedList implementation

        public int Page { get; }

        public int PageSize { get; }

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page * PageSize < TotalRecords;

        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        public int TotalRecords { get; }

        public IList<T> Records => _data;

        #endregion IPagedList implementation

        #region List implementation

        public void AddRange(IEnumerable<T> range)
        {
            _data.AddRange(range);
        }

        public int IndexOf(T item)
        {
            return _data.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _data.RemoveAt(index);
        }

        public T this[int index]
        {
            get => _data[index];
            set => _data[index] = value;
        }

        public void Add(T item)
        {
            _data.Add(item);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(T item)
        {
            return _data.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        public int Count => _data.Count();

        public bool IsReadOnly => true;

        public bool Remove(T item)
        {
            return _data.Remove(item);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)_data).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)_data).GetEnumerator();
        }

        #endregion List implementation
    }
}