using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.App.Extensions
{
    public class PaginatedList<T> : List<T>
    {
        public int TotalCount { get; private set; }
        public int PageSize { get; private set; }
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int totalPages,  int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize); 
            TotalPages= totalPages;
            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync(); 
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return new PaginatedList<T>(items, count, totalPages, pageIndex, pageSize );
        }
    }
}
