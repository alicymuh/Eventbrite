using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApi.Domain
{
    public class EventItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }        
        public string Description { get; set; }       
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
        public virtual Logo Logo{ get; set; }
        public virtual Venue Venue { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Format Format { get; set; }
        public virtual Category Category { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

    }
}
