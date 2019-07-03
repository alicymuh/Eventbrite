using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventbriteMvc.Services;
using EventbriteMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace EventbriteMvc.Controllers
{
    public class EventController : Controller
    {
        private IEventService _eventSvc;
        private readonly ILogger _logger;/*new*/

        public EventController(IEventService eventSvc, ILoggerFactory logger) {
            _eventSvc = eventSvc;
            _logger = logger.CreateLogger<EventController>();
        }
        public async Task<IActionResult> Index(int? CategoryFilterApplied,int? page)
        {
            
            int itemsPage = 10;
            var eventItem = await _eventSvc.GetEventAsync(page ?? 0, itemsPage, CategoryFilterApplied);

            if (eventItem == null)
            {
                _logger.LogDebug("the eventIten returned is null");
            }
           // _logger.LogDebug("the categoryId chose by user is" + eventItem.CategoryId);
            var v_model = new EventIndexViewModel()
            {
                EventItems = eventItem.Data,
                Categories = await _eventSvc.GetCategory(),
               // Prices = await _eventSvc.GetPrice(),
                CategoryFilterApplied = CategoryFilterApplied ?? 0,
               // PriceFilterApplied = PriceFilterApplied ?? 0,

                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = itemsPage,
                    TotalItems = eventItem.Count,
                    TotalPages = (int)Math.Ceiling(((decimal)eventItem.Count / itemsPage))
                }
            };

            v_model.PaginationInfo.Next = (v_model.PaginationInfo.ActualPage == v_model.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            v_model.PaginationInfo.Previous = (v_model.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            return View(v_model);
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";


            return View();
        }
    }
}