using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventbriteMvc.Infrastructure
{
    public class ApiPaths
    {
        public static class Event
        {
            public static string GetAllEventItems(string baseUri, int page,
                int take, int? category)
            {
                var filterQs = string.Empty;

                if(category.HasValue)
                {
                    var categoryQ = (category.HasValue) ? category.Value.ToString() : "null";
                    //var priceQ = (price.HasValue) ? price.Value.ToString() : "null";
                    filterQs = $"/category/{categoryQ}";
                }

                return $"{baseUri}EventItems{filterQs}?pageIndex={page}&pageSize={take}";
            }

            public static string GetEventItem(string baseUri, int id)
            {
                return $"{baseUri}/event/{id}";
            }        
            public static string GetAllCategories (string baseUri)
            {
                return $"{baseUri}categories";
            }
        }

        public static class Basket
        {
            public static string GetBasket(string baseUri, string basketId)
            {
                return $"{baseUri}/{basketId}";
            }

            public static string UpdateBasket(string baseUri)
            {
                return baseUri;
            }

            public static string CleanBasket(string baseUri, string basketId)
            {
                return $"{baseUri}/{basketId}";
            }
        }

        public static class Order
        {
            public static string GetOrder(string baseUri, string orderId)
            {
                return $"{baseUri}/{orderId}";
            }

            public static string GetOrders(string baseUri)
            {
                return baseUri;
            }

            public static string AddNewOrder(string baseUri)
            {
                return $"{baseUri}/new";
            }

        }
    }
}
