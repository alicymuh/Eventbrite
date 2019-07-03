using EventApi.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApi.Data
{
    public class EventSeed
    {
        public static async Task SeedAsync(EventContext context)
        {
            context.Database.Migrate();
            if (!context.Categories.Any())
            {
                context.Categories.AddRange
                    (GetPreconfiguredCategories());
                context.SaveChanges();
            }
            
            if (!context.Logos.Any())
            {
                context.Logos.AddRange
                    (GetPreconfiguredLogos());
                context.SaveChanges();
            }
            if (!context.Organizations.Any())
            {
                context.Organizations.AddRange
                    (GetPreconfiguredOrganizations());
                context.SaveChanges();
            }
            if (!context.Formats.Any())
            {
                context.Formats.AddRange
                    (GetPreconfiguredFormats());
                context.SaveChanges();
            }
            if (!context.Addresses.Any())
            {
                context.Addresses.AddRange
                    (GetPreconfiguredAddresses());
                context.SaveChanges();
            }
            if (!context.Venues.Any())
            {
                context.Venues.AddRange
                    (GetPreconfiguredVenues());
                await context.SaveChangesAsync();
            }
            if (!context.EventItems.Any())
            {
                 context.EventItems.AddRange
                     (GetPreconfiguredEvents());
                 context.SaveChanges();
            }


        }

        static IEnumerable<Category> GetPreconfiguredCategories()
        {
            return new List<Category>()
            {
                new Category() { Name = "Science & Tech", CategoryResourceUrl = "https://www.eventbriteapi.com/v3/categories/103/"},
                new Category() { Name = "Business", CategoryResourceUrl = "https://www.eventbriteapi.com/v3/categories/103/" },
                new Category() { Name = "Charity & Causes", CategoryResourceUrl = "https://www.eventbriteapi.com/v3/categories/103/" },
                new Category() { Name = "Community", CategoryResourceUrl = "https://www.eventbriteapi.com/v3/categories/103/"},
                new Category() { Name = "Family & Education", CategoryResourceUrl = "https://www.eventbriteapi.com/v3/categories/103/"},
                new Category() { Name = "Fashion", CategoryResourceUrl = "https://www.eventbriteapi.com/v3/categories/103/" },
                new Category() { Name = "Film & Media", CategoryResourceUrl = "https://www.eventbriteapi.com/v3/categories/103/"},
                new Category() { Name = "Food & Drink", CategoryResourceUrl = "https://www.eventbriteapi.com/v3/categories/103/" },
                new Category() { Name = "Government", CategoryResourceUrl = "https://www.eventbriteapi.com/v3/categories/103/" },
                new Category() { Name = "Health", CategoryResourceUrl = "https://www.eventbriteapi.com/v3/categories/103/"},
                new Category() { Name = "Home & Lifestyle", CategoryResourceUrl = "https://www.eventbriteapi.com/v3/categories/103/" },
                new Category() { Name = "Music", CategoryResourceUrl = "https://www.eventbriteapi.com/v3/categories/103/" }

            };
        }

        static IEnumerable<Address> GetPreconfiguredAddresses()
        {
            return new List<Address>()
            {
                new Address() { Address_1 = "11100 NE 6th St", City="Bellevue", State = "Washington", Postal_Code="98004", Country="USA"},
                new Address() { Address_1 = "115 Bell Street", City="Seattle", State = "Washington", Postal_Code="98121", Country="USA" },
                new Address() { Address_1 = "6046 West Lake Sammamish Pkwy NE ", City="Redmond", State = "Washington", Postal_Code="98052", Country="USA" },
                new Address() { Address_1 = "25300 Miller Bay Road Northeast ", City="Kingston", State = "Kingston", Postal_Code="98346 ", Country="USA"},
                new Address() { Address_1 = "9205 Airport Rd ", City="Everett", State = "Washington", Postal_Code="98004", Country="USA"}
            };

        }

        static IEnumerable<Organization> GetPreconfiguredOrganizations()
        {
            return new List<Organization>()
            {
                new Organization() { Name = "Awesome Org"},
                new Organization() { Name = "SupplyChain Org" },
                new Organization() { Name = "Party Org" },
                new Organization() { Name = "Events Org"},
                new Organization() { Name = "Cultures Org"}
            };

        }

        static IEnumerable<Logo> GetPreconfiguredLogos()
        {
            return new List<Logo>()
            {
                new Logo() { LogoUrl = "https://image.com"},
                new Logo() { LogoUrl = "https://image.com" },
                new Logo() { LogoUrl = "https://image.com" },
                new Logo() { LogoUrl = "https://image.com"}
            };

        }

        static IEnumerable<Format> GetPreconfiguredFormats()
        {
            return new List<Format>()
            {
                new Format() { Name = "Seminar", FormatResourceUrl="https://www.eventbriteapi.com/v3/formats/2/"},
                new Format() { Name = "Talk", FormatResourceUrl="https://www.eventbriteapi.com/v3/formats/2/" },
                new Format() { Name = "Concert", FormatResourceUrl="https://www.eventbriteapi.com/v3/formats/3/" },
                new Format() { Name = "Workshop", FormatResourceUrl="https://www.eventbriteapi.com/v3/formats/5/"},
                new Format() { Name = "Food", FormatResourceUrl="https://www.eventbriteapi.com/v3/formats/6/"}
            };

        }

        static IEnumerable<Venue> GetPreconfiguredVenues()
        {
            return new List<Venue>()
            {
                new Venue () { Name= "Capitol Hill Block Party", Capacity=200, VenueResourceUrl="https://www.eventbriteapi.com/v3/venues/3003/", AddressId=2},
                new Venue() { Name=  "King County's Marymoor Park", Capacity=130, VenueResourceUrl="https://www.eventbriteapi.com/v3/venues/2000/", AddressId=3 },
                new Venue() { Name= "Meydenbauer Convention Center", Capacity=320, VenueResourceUrl="https://www.eventbriteapi.com/v3/venues/2050/", AddressId=1 },
                new Venue() { Name= "Kingston House", Capacity=400, VenueResourceUrl="https://www.eventbriteapi.com/v3/venues/1234/", AddressId=4},
                new Venue() { Name= "Green Lake Restaurants & Businesses", Capacity=670, VenueResourceUrl="https://www.eventbriteapi.com/v3/venues/1040/", AddressId=5}              

            };
        }

             
        static IEnumerable<EventItem> GetPreconfiguredEvents()
        {
            return new List<EventItem>()
            {
                new EventItem() { Name = "Wanderlust Seattle 2019", Summary="festival tour", Description="we’re bringing our destination festival experience to select US cities", Start=new DateTime(2019,05,17), End=new DateTime(2019,05,17), Created=new DateTime(2019,05,17), Changed=new DateTime(2019,05,17), Published=new DateTime(2019,05,17),Status="Live",Online_event=false,CategoryId=4,VenueId=2, OrganizationId=4, LogoId=1,FormatId=1, PictureUrl="http://externalcatalogbaseurltobereplaced/api/pic/4", Price=154},
                new EventItem() { Name = "Capitol Hill Block Party 2019" , Summary="Festival Party", Description="Music,Dance", Start=new DateTime(2019,05,24), End=new DateTime(2019,05,24),Created=new DateTime(2019,05,24), Changed=new DateTime(2019,05,24), Published=new DateTime(2019,05,24), Status="Live",Online_event=false,CategoryId=12,VenueId=1, OrganizationId=4, LogoId=2,FormatId=2, PictureUrl="http://externalcatalogbaseurltobereplaced/api/pic/1", Price=135},
                new EventItem() { Name = "14th Annual Washington Brewer’s Festiva" , Summary="Washington Brewer’s Festival", Description="The Washington Brewer’s Festival is a unique all-ages beer festival", Start=new DateTime(2019,05,11), End=new DateTime(2019,05,11), Created=new DateTime(2019,05,11), Changed=new DateTime(2019,05,11), Published=new DateTime(2019,05,11),Status="Live",Online_event=false,CategoryId=8,VenueId=4, OrganizationId=3, LogoId=3,FormatId=5, PictureUrl="http://externalcatalogbaseurltobereplaced/api/pic/2", Price=45},
                new EventItem() { Name = "SEATTLE- The Cheese and Meat Festival" , Summary="Cheese and Meat Festival", Description="The Cheese and Meat Festival allows consumers to taste their way through international and local artisan foods in sample bites", Start=new DateTime(2019,05,12), End=new DateTime(2019,05,12), Created=new DateTime(2019,05,12), Changed=new DateTime(2019,05,12), Published=new DateTime(2019,05,12),Status="Live",Online_event=false,CategoryId=8,VenueId=5, OrganizationId=3, LogoId=4,FormatId=5, PictureUrl="http://externalcatalogbaseurltobereplaced/api/pic/3", Price=79},
                new EventItem() { Name = "The 3rd Annual Bellevue Tech Expo" , Summary="Tech Expo", Description="The 3rd Annual Bellevue Tech Expo is a showcase of Tech, Talent, and Innovation in the Northwest", Start=new DateTime(2019,05,19), End=new DateTime(2019,05,19), Created=new DateTime(2019,05,19), Changed=new DateTime(2019,05,19), Published=new DateTime(2019,05,19),Status="Live",Online_event=false,CategoryId=1,VenueId=3, OrganizationId=4, LogoId=4,FormatId=2, PictureUrl="http://externalcatalogbaseurltobereplaced/api/pic/5", Price=200 }
            };

        }

    }
}
