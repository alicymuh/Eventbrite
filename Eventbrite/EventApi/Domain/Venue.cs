using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApi.Domain
{
    public class Venue
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public string Name { get; set; }
        public string VenueResourceUrl { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }


    }
}
