using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventbriteMvc.Infrastructure;
using EventbriteMvc.Models.EventModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventbriteMvc.Services
{
    public class EventService : IEventService
    {
        private readonly IHttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;
        private readonly IOptionsSnapshot<AppSettings> _settings;

        public EventService(IOptionsSnapshot<AppSettings> settings, IHttpClient httpClient)
        {
            _settings = settings;
            _apiClient = httpClient;
            _remoteServiceBaseUrl = $"{_settings.Value.EventUrl}/api/event/";
        }
        public async Task<Event> GetEventAsync(int page, int take, int? category)
        {
            var alleventItemsUri = ApiPaths.Event.GetAllEventItems(_remoteServiceBaseUrl, page, take, category);

            string dataContent = await _apiClient.GetStringAsync(alleventItemsUri);
            var response = JsonConvert.DeserializeObject<Event>(dataContent.ToString());
            return response;
        }

        public async Task<IEnumerable<SelectListItem>> GetCategory()
        {
            var getCategoryUri = ApiPaths.Event.GetAllCategories(_remoteServiceBaseUrl);
            var dataContent = await _apiClient.GetStringAsync(getCategoryUri);

            var categoryItem = new List<SelectListItem>
            {
                new SelectListItem() { Value= null, Text="All", Selected = true}
            };
            var categories = JArray.Parse(dataContent);

            foreach(var category in categories.Children<JObject>())
            {
                categoryItem.Add(new SelectListItem()
                {
                    Value = category.Value<string>("id"),
                    Text = category.Value<string>("name")
                });
            }
            return categoryItem;

        }      
        public Task<IEnumerable<SelectListItem>> GetPrice()
        {
            throw new NotImplementedException();
        }
    }
}
