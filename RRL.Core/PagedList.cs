using System.Collections.Generic;
using System.Linq;

namespace RRL.Core
{
    public interface IPagedList
    {
        int TotalCount { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }
    }

    public class PagedList<T> : List<T>, IPagedList
    {
        public PagedList(IQueryable<T> source, int index, int pageSize)
        {
            if (index < 1) { index = 1; }
            /**/
            TotalCount = source.Count();
            PageSize = pageSize;
            PageIndex = index;
            AddRange((IEnumerable<T>)source.Skip((index - 1) * pageSize).Take(pageSize));
        }

        public PagedList(IEnumerable<T> source, int total, int index, int pageSize)
        {
            if (index < 1) { index = 1; }
            /**/
            TotalCount = total;
            PageSize = pageSize;
            PageIndex = index;
            AddRange(source);
        }

        public int PageTotal
        {
            get
            {
                return (TotalCount + PageSize - 1) / PageSize;
            }
        }

        public int TotalCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}