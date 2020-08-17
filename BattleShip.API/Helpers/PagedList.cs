using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            this.TotalCount = count;
            this.PageSize = pageSize;
            this.CurrentPage = pageNumber;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public static PagedList<T> Create(List<T> source, int pageNumber, int pageSIze)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSIze).Take(pageSIze).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSIze);
        }
    }
}
