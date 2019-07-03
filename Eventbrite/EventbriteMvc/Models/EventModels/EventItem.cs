using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventbriteMvc.Models.EventModels
{
    public class EventItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime Created { get; set; }
        public DateTime Changed { get; set; }
        public DateTime Published { get; set; }
        public string Status { get; set; }
        public bool Online_event { get; set; }
        public int LogoId { get; set; }
        public int VenueId { get; set; }
        public int OrganizationId { get; set; }
        public int FormatId { get; set; }
        public int CategoryId { get; set; }
        public string Logo { get; set; }
        public string Venue { get; set; }
        public string Organization { get; set; }
        public string Format { get; set; }
        public string Category { get; set; }
    }
}
