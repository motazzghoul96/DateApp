using System.Text.Json;
using API.Helper;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHearder(this HttpResponse response,
        int currentPage,int itemsPerPage,int totalItem,int totalPages)
        {
            var paginationHeader=new PaginationHeader(currentPage,itemsPerPage,totalItem,totalPages);
            var options=new JsonSerializerOptions
            {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase
            };
            response.Headers.Add("Pagination",JsonSerializer.Serialize(paginationHeader,options));
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
            
        }
    }
}