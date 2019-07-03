using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventbriteMvc.Models;
using EventbriteMvc.ViewModels;
using EventbriteMvc.Services;

namespace EventbriteMvc.Controllers
{
    public class HomeController : Controller
    {
        private IEventService _eventSvc;

        public HomeController(IEventService eventSvc) =>
            _eventSvc = eventSvc;
        public async Task<IActionResult> Index(int? CategoryFilterApplied, int? page)
        {
            int itemsPage = 10;
            var eventItem = await _eventSvc.GetEventAsync(page ?? 0, itemsPage, CategoryFilterApplied);

            var v_model = new EventIndexViewModel()
            {
                EventItems = eventItem.Data,
                Categories = await _eventSvc.GetCategory(),
              //  Prices = await _eventSvc.GetPrice(),
                CategoryFilterApplied = CategoryFilterApplied ?? 0,
                //PriceFilterApplied = PriceFilterApplied ?? 0,

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
        /*public IActionResult Index()
        {
            return View();
        }*/

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
