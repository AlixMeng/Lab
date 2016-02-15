using System;
using System.Collections.Generic;

namespace LabRequest.DomainModel.Repository
{
    public class PagedData<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _currentItems;
        public int TotalCount { get; private set; }
        public int Page { get; private set; }
        public int PerPage { get; private set; }
        public int TotalPage { get; private set; }
        public bool HasNextPage { get; private set; }
        public bool HasPreviousPage { get; private set; }
        public int Last { get; set; }
        public int First { get; set; }
        
        public PagedData(IEnumerable<T> currentItems, int totalCount, int page, int perPage)
        {
            this._currentItems = currentItems;
            TotalCount = totalCount;
            Page = page;
            PerPage = perPage;
            TotalPage = (int)Math.Ceiling((float)TotalCount / PerPage);
            HasNextPage = Page < TotalPage;
            HasPreviousPage = Page > 1;
        }


        public int NextPage
        {
            get
            {
                if (!HasNextPage) throw new InvalidOperationException();
                return Page + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (!HasPreviousPage) throw new InvalidOperationException();
                return Page - 1;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _currentItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
