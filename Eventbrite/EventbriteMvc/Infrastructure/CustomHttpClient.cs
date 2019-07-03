using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EventbriteMvc.Infrastructure
{
    public class CustomHttpClient : IHttpClient
    {
        private HttpClient _client;
        private ILogger<CustomHttpClient> _logger;
        private IHttpContextAccessor _httpContextAccesor;
        
        public CustomHttpClient (ILogger<CustomHttpClient> logger, IHttpContextAccessor httpContextAccesor)
        {
            _client = new HttpClient();
            _logger = logger;
            _httpContextAccesor = httpContextAccesor;

        }
        public async Task<string> GetStringAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            if (authorizationToken != null)
            {
               requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);

            }

            var response = await _client.SendAsync(requestMessage);

            return await response.Content.ReadAsStringAsync();
            //throw new NotImplementedException();
        }
        public async Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            //SetAuthorizationHeader(requestMessage);
            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }
            return await _client.SendAsync(requestMessage);
        }        

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return await DoPostPutAsync(HttpMethod.Put, uri, item, authorizationToken, authorizationMethod);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string uri, T item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return await DoPostPutAsync(HttpMethod.Put, uri, item, authorizationToken, authorizationMethod);
        }

        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string uri, T item, string authorizationToken=null, string authorizationMethod = "Bearer")
        {
            if(method != HttpMethod.Post && method!= HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put", nameof(method));
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            var cartToInsert = JsonConvert.SerializeObject(item);
            _logger.LogDebug("This is the cart data we're going to post to CartApi" + cartToInsert);

            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");

            //SetAuthorizationHeader(requestMessage);
            if(authorizationToken!= null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            var response = await _client.SendAsync(requestMessage);

            //raise exception if HttpResponseCode 500
            //needed for ciruit breaker to track fails

            if(response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }
            return response;
        }

        private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {
            var authorizationHeader = _httpContextAccesor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                requestMessage.Headers.Add("Authorization", new List<string> { authorizationHeader });
            }

        }
    }
}
