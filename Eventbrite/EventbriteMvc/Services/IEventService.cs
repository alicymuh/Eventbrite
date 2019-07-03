using EventbriteMvc.Models.EventModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventbriteMvc.Services
{
    public interface IEventService
    {
        Task<Event> GetEventAsync(int page, int take, int? category);
        //Task<IEnumerable<Event>> GetEventAsync(int page, int take, int? category, int? price);

        Task<IEnumerable<SelectListItem>> GetCategory();

        Task<IEnumerable<SelectListItem>> GetPrice();
    }
}
