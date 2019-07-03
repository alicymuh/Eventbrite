using EventApi.Data;
using EventApi.Domain;
using EventApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Event")]
    public class EventController: Controller
    {

        private readonly EventContext _eventContext;
        private readonly IOptionsSnapshot<EventSettings> _settings;

        public EventController(EventContext eventContext, IOptionsSnapshot<EventSettings> settings)
        {
            _eventContext = eventContext;
            _settings = settings;
            
        }
        /*[HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        /* [HttpGet]
         [Route("[action]")]
         public async Task<IActionResult> EventItems()
         {
             var items = await _eventContext.Categories.ToListAsync();
             return Ok(items);

         }*/
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Categories()
        {
            var items = await _eventContext.Categories.ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> EventItems(
            [FromQuery] int pageSize = 6,
            [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _eventContext.EventItems
                              .LongCountAsync();
            var itemsOnPage = await _eventContext.EventItems
                              .OrderBy(c => c.Name)
                              .Skip(pageSize * pageIndex)
                              .Take(pageSize)
                              .ToListAsync();            
             itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);

            var model = new PaginatedItemsViewModel<EventItem>
                (pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        [HttpGet]
        [Route("[action]/category/{categoryId}")]
        public async Task<IActionResult> ItemsCategory(int? categoryId)
        {
            var root = (IQueryable<EventItem>)_eventContext.EventItems;

            if (categoryId.HasValue)
            {
                root = root.Where(c => c.CategoryId == categoryId);
            }
           
            /*var totalItems = await root
                              .LongCountAsync();*/
            var itemsOnPage = root.ToArray();
           /* itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);*/

            return Ok(itemsOnPage);

        }

        // GET api/Event/Items/Category/1/Price/null[?pageSize=4&pageIndex=0]
        [HttpGet]
        [Route("[action]/category/{eventCategoryId}")]
        public async Task<IActionResult> Items(int? eventCategoryId,
            [FromQuery] int pageSize = 6,
            [FromQuery] int pageIndex = 0)
        {
            var root = (IQueryable<EventItem>)_eventContext.EventItems;

            if (eventCategoryId.HasValue)
            {
                root = root.Where(c => c.CategoryId== eventCategoryId);
            }

            var totalItems = await root
                              .LongCountAsync();
            var itemsOnPage = await root
                              .OrderBy(c => c.Name)
                              .Skip(pageSize * pageIndex)
                              .Take(pageSize)
                              .ToListAsync();
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);
            var model = new PaginatedItemsViewModel<EventItem>(pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(model);

        }

        private List<EventItem> ChangeUrlPlaceHolder(List<EventItem> items)
        {
            items.ForEach(
                x => x.PictureUrl =
                x.PictureUrl
                .Replace("http://externalcatalogbaseurltobereplaced",
                _settings.Value.ExternalEventBaseUrl));
            return items;
        }


    }
}
