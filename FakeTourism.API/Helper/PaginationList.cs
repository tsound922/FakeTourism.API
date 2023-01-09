using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTourism.API.Helper
{
    public class PaginationList<T>: List<T>
    {
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public PaginationList(int totalCount, int currentPage, int pageSize, List<T> items) 
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            AddRange(items);
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        //Factory PATTERN
        public async static Task<PaginationList<T>> CreateAsync(
            int currentPage, 
            int pageSize,
            IQueryable<T> result
            ) 
        {
            var totalCount = await result.CountAsync();

            //pagination
            //1 Skip some data query
            var skip = (currentPage - 1) * pageSize;
            result = result.Skip(skip);

            //2 list data amount based on pagesize
            result = result.Take(pageSize);

            var item = await result.ToListAsync();


            return new PaginationList<T>(totalCount, currentPage, pageSize, item);
        }

    }
}
