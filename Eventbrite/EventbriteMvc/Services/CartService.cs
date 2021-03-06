﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventbriteMvc.Infrastructure;
using EventbriteMvc.Models.CartModel;
using EventbriteMvc.Models.OrderModels;
using EventbriteMvc.Models.UserModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EventbriteMvc.Services
{
    public class CartService : ICartService
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private IHttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;
        private IHttpContextAccessor _httpContextAccesor;
        private readonly ILogger _logger;

        public CartService(IOptionsSnapshot<AppSettings> settings, IHttpContextAccessor httpContextAccesor, IHttpClient httpClient, ILoggerFactory logger)
        {
            _settings = settings;
            _remoteServiceBaseUrl = $"{settings.Value.CartUrl}/api/v1/cart";
            _httpContextAccesor = httpContextAccesor;
            _apiClient = httpClient;
            _logger = logger.CreateLogger<CartService>();
        }
        public async Task AddItemToCart(ApplicationUser user, CartItem eventItem)
        {
            var cart = await GetCart(user);
            _logger.LogDebug("User Name: " + user.Id);
            _logger.LogDebug("Unit Price is :" + eventItem.UnitPrice);

            if(cart == null)
            {
                _logger.LogDebug("The cart is still empty for  user Name:" + user.Id);
                cart = new Cart()
                {
                    BuyerId = user.Id,
                    Events = new List<CartItem>()
                };
            }

            var basketItem = cart.Events
                .Where(e => e.EventId == eventItem.EventId)
                .FirstOrDefault();

            if(basketItem == null)
            {
                _logger.LogInformation("the basket is null now");
                cart.Events.Add(eventItem);
                foreach(var eventName in cart.Events)
                {
                    _logger.LogInformation(eventName.EventName);
                }
                _logger.LogInformation("the event id added is" + eventItem.EventId);
                _logger.LogInformation("the event added is" + eventItem.EventName);
            }
            else
            {
                basketItem.Quantity += 1;
                _logger.LogInformation("the quantity added is :" + basketItem.Quantity);
            }
            await UpdateCart(cart);
        }

        public async Task ClearCart(ApplicationUser user)
        {
            var token = await GetUserTokenAsync();
            var cleanBasketUri = ApiPaths.Basket.CleanBasket(_remoteServiceBaseUrl, user.Id);
            _logger.LogDebug("Clean Basket uri: " + cleanBasketUri);
            var response = await _apiClient.DeleteAsync(cleanBasketUri);
            _logger.LogDebug("Basket cleaned");
            
        }

        public async Task<Cart> GetCart(ApplicationUser user)
        {
            var token = await GetUserTokenAsync();
            _logger.LogInformation("this is a get basket method and user id" + user.Id);
            _logger.LogInformation(_remoteServiceBaseUrl);

            var getBasketUri = ApiPaths.Basket.GetBasket(_remoteServiceBaseUrl, user.Id);
            _logger.LogInformation(getBasketUri);
            var dataString = await _apiClient.GetStringAsync(getBasketUri, token);
            _logger.LogInformation(dataString);

            var response = JsonConvert.DeserializeObject<Cart>(dataString.ToString()) ??
              new Cart()
              {
                  BuyerId = user.Id
              };
            _logger.LogInformation(response.ToString());
            return response;
        }

        public async Task<Cart> SetQuantities(ApplicationUser user, Dictionary<string, int> quantities)
        {
            var basket = await GetCart(user);

            basket.Events.ForEach(x =>
            {
                //Simplify this logic by using the new out variable
                // initializer
                if (quantities.TryGetValue(x.Id, out var quantity))
                {
                    x.Quantity = quantity;
                }
            });
            return basket;
        }

        public async Task<Cart> UpdateCart(Cart cart)
        {
            var cartToInsert = JsonConvert.SerializeObject(cart);
            _logger.LogDebug("This is the cart data we want to insert" + cartToInsert);
            var token = await GetUserTokenAsync();
            _logger.LogDebug("Service url: " + _remoteServiceBaseUrl);
            var updateBasketUri = ApiPaths.Basket.UpdateBasket(_remoteServiceBaseUrl);
            _logger.LogDebug("Update Basket url:" + updateBasketUri);
            var response = await _apiClient.PostAsync(updateBasketUri, cart, token);
            
            _logger.LogDebug(await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();

            return cart;
        }

        async Task<string> GetUserTokenAsync()
        {
            var context = _httpContextAccesor.HttpContext;
            return await context.GetTokenAsync("access_token");
        }

        public Order MapCartToOrder(Cart cart)
        {
            var order = new Order();
            order.OrderTotal = 0;

            cart.Events.ForEach(x =>
            {
                order.OrderItems.Add(new OrderItem()
                { 
                    EventId = int.Parse(x.EventId),
                    PictureUrl = x.PictureUrl,
                    EventName = x.EventName,
                    Units = x.Quantity,
                    UnitPrice = x.UnitPrice

                });
                order.OrderTotal += (x.Quantity * x.UnitPrice);

            });
            return order;
        }
    }
}
