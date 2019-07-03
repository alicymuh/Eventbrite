using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using StackExchange.Redis;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CartApi.Model
{
    public class RedisCartRepository : ICartRepository
    {
        private readonly ILogger<RedisCartRepository> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisCartRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis)
        {
            _logger = loggerFactory.CreateLogger<RedisCartRepository>();
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteCartAsync (string id)
        {
            return await _database.KeyDeleteAsync(id);
        }

        public IEnumerable<string> GetUsers()
        {
            var endpoint = _redis.GetEndPoints();
            var server = _redis.GetServer(endpoint.First());
            var data = server.Keys();
            return data?.Select(k => k.ToString());

        }

        public async Task<Cart> GetCartAsync(string customerId)
        {
            var data = await _database.StringGetAsync(customerId);
            if (data.IsNullOrEmpty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Cart>(data);
        }

        public async Task<Cart> UpdateCartAsync(Cart basket)
        {
           /* List<CartEvent> l = new List<CartEvent>();

            CartEvent sample = new CartEvent()
            {
                Id = "3",
                EventId = "56",
                EventName = "waterlust",
                UnitPrice = 2,
                OldUnitPrice = 3,
                Quantity = 10,
                PictureUrl = "this the url"
            };

            l.Add(sample);
            string cartId = "3";

            Cart testbasket = new Cart(cartId)
            {
                BuyerId = cartId,
                Events = l
            };
            var created = await _database.StringSetAsync(testbasket.BuyerId, JsonConvert.SerializeObject(testbasket));
            var valueresp = await _database.StringGetAsync(testbasket.BuyerId);
            return JsonConvert.DeserializeObject<Cart>(valueresp);*/
            var created = await _database.StringSetAsync(basket.BuyerId, JsonConvert.SerializeObject(basket));
            if (!created)
            {
                _logger.LogInformation("Problem occur persisting the item.");
                return null;
            }

            _logger.LogInformation("Basket item persisted succesfully.");

            return await GetCartAsync(basket.BuyerId);


            /*  _logger.LogInformation("got to update basket cart method");
            var created = await _database.StringSetAsync(basket.BuyerId, JsonConvert.SerializeObject(basket));
            if (!created)
            {
                _logger.LogInformation("Problem occur persisting the item");
                return null;
            }

            _logger.LogInformation("Basket item persisted succesfully");
            return await GetCartAsync(basket.BuyerId);*/
        }

        /* public async Task<List<string>> CheckPersistency()
         {
             string buyerId = "2";
             List<string> l = new List<string> { "try", "to", "see", "if", "redis", "works" };
             var created = await _database.StringSetAsync(buyerId, JsonConvert.SerializeObject(l));
             var valueresp = await _database.StringGetAsync(buyerId);
             return JsonConvert.DeserializeObject<List<String>>(valueresp);
         }*/

        
       /* public async Task<string> CheckPersistency(Cart basket)
        {
            var created = await _database.StringSetAsync(basket.BuyerId, JsonConvert.SerializeObject(basket.BuyerId));
            var valueresp = await _database.StringGetAsync(basket.BuyerId);
            return JsonConvert.DeserializeObject<String>(valueresp);
        }*/

       public async Task<Cart> CheckPersistency(Cart basket)
        {
            var created = await _database.StringSetAsync(basket.BuyerId, JsonConvert.SerializeObject(basket));
            var valueresp = await _database.StringGetAsync(basket.BuyerId);
            return JsonConvert.DeserializeObject<Cart>(valueresp);
        }

        /*public async Task<string> CheckPersistency(Cart basket)
        {
            var created = await _database.StringSetAsync(basket.BuyerId, JsonConvert.SerializeObject(basket));
            var valueresp = await _database.StringGetAsync(basket.BuyerId);
            return JsonConvert.DeserializeObject<Cart>(valueresp);
        }*/
    }
}
