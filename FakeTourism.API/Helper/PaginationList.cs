using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTourism.API.Helper
{
    public class PaginationList<T>: List<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public PaginationList(int currentPage, int pageSize, List<T> items) 
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            AddRange(items);
        }

        //Factory PATTERN
        public async static Task<PaginationList<T>> CreateAsync(
            int currentPage, 
            int pageSize,
            IQueryable<T> result
            ) 
        {
            //pagination
            //1 Skip some data query
            var skip = (currentPage - 1) * pageSize;
            result = result.Skip(skip);

            //2 list data amount based on pagesize
            result = result.Take(pageSize);

            var item = await result.ToListAsync();


            return new PaginationList<T>(currentPage, pageSize, item);
        }

    }
}
