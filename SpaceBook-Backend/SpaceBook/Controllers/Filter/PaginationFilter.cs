using System;
namespace SpaceBook.Controllers.Filter
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 20;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 40 ? 40 : pageSize;
        }
    }
}
