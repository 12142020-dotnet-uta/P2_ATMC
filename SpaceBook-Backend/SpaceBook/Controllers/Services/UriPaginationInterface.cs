using System;
using SpaceBook.Controllers.Filter;

namespace SpaceBook.Controllers.Services
{
    public interface UriPaginationInterface
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
