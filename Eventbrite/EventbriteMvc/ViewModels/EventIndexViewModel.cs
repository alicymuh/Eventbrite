using EventbriteMvc.Models.EventModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventbriteMvc.ViewModels
{
    public class EventIndexViewModel
    { 
        public IEnumerable<EventItem> EventItems { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        //public IEnumerable<SelectListItem> Prices { get; set; }
        public int? CategoryFilterApplied { get; set; }
        public int? PriceFilterApplied { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
