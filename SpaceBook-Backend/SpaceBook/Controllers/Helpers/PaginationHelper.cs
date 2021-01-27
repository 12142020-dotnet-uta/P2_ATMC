using System;
using System.Collections.Generic;
using SpaceBook.Controllers.Filter;
using SpaceBook.Controllers.Services;
using SpaceBook.Controllers.Wrappers;

namespace SpaceBook.Controllers.Helpers
{
    public class PaginationHelper
    {

        public static PagedResponse<List<T>> CreatePagedResponse<T>(List<T> pagedData, PaginationFilter validFilter, int records, UriPaginationInterface uriService, string route)
        {
            var res = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var pages = ((double)records / (double)validFilter.PageSize);

            int roundedPages = Convert.ToInt32(Math.Ceiling(pages));
            res.Next =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                : null;
            res.Previous =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                : null;
            res.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);
            res.LastPage = uriService.GetPageUri(new PaginationFilter(roundedPages, validFilter.PageSize), route);
            res.TotalPages = roundedPages;
            res.TotalRecords = records;
            return res;
            ;
        }

    }
}
