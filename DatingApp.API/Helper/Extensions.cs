using System;
using DatingApp.API.Helper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DatingApp.API.Helper {
    public static class Extensions {
        public static void AddApplicationError (this HttpResponse response, string message) {
            response.Headers.Add ("Application-Error", message);
            response.Headers.Add ("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add ("Access-Control-Allow-Origin", "*");
        }
        public static void AddPagination (this HttpResponse response, int curentPage, int itemsPerPage, int totalItems, int totalPages) {
            var paginationHeader = new PaginationHeader (curentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings ();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver ();
            response.Headers.Add ("Pagination", JsonConvert.SerializeObject (paginationHeader, camelCaseFormatter));
            response.Headers.Add ("Access-Control-Expose-Headers", "Pagination");
            response.Headers.Add ("Access-Control-Allow-Origin", "*");
        }
        public static int CalculateAge (this DateTime thedatetime) {
            var age = DateTime.Today.Year - thedatetime.Year;

            if (thedatetime.AddYears (age) > DateTime.Today)
                age--;
            return age;
        }
    }
}