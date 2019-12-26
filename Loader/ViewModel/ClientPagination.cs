using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.ViewModel
{
    public class Pagination<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public Pagination(IQueryable<T> source, int pageIndex, int pageSize,int totalCount=0)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            if(totalCount!=0)
            {
                TotalCount = totalCount;
            }
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            if (pageIndex != 1)
            {
                int pageFinal = pageIndex - 1;
                this.AddRange(source.Skip(pageFinal * PageSize).Take(pageSize));
                //source ma vako sab skip va vayera mathi ko code modify gareko
                //this.AddRange(source.Take(pageSize));
            }
            else
            {
                this.AddRange(source.Take(PageSize));
            }
        }
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return (PageIndex + 1 < TotalPages);
            }
        }
    }
}