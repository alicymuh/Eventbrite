using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventbriteMvc.Models.CartModel;
using EventbriteMvc.Models.EventModels;
using EventbriteMvc.Models.UserModel;
using EventbriteMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;

namespace EventbriteMvc.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IEventService _eventService;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly ILogger _logger;/*new*/


        public CartController(IIdentityService<ApplicationUser> identityService, ICartService cartService, IEventService eventService, ILoggerFactory logger)
        {
            _cartService = cartService;
            _eventService = eventService;
            _identityService = identityService;
            _logger = logger.CreateLogger<CartController>();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Dictionary<string, int> quantities,string action)
        {
            /*if(action == "[ Checkout ]")
            {
                return RedirectToAction("Index","Event");
            }
           
            //return "This is Index action the Mvc-CartController";*/
            // _logger.LogDebug("The action pressed is" + action);
            //return RedirectToAction("Create", "Order");

            /*if (action == null || string.IsNullOrEmpty(action))
            {
                return RedirectToAction("Create", "Order");
            }*/
            if (action == "[ Checkout ]")
            {
                return RedirectToAction("Create", "Order");
            }

            try
            {
                var user = _identityService.Get(HttpContext.User);
                var basket = await _cartService.SetQuantities(user, quantities);
                var vm = await _cartService.UpdateCart(basket);
            }
            catch(BrokenCircuitException)
            {
                // Catch error when CartApi is in open circuit  mode                 
                HandleBrokenCircuitException();
            }
            return View();
        }



        public async Task<IActionResult> AddToCart(EventItem eventDetails)
        {
            try
            {
                if (eventDetails.Id != null)
                {
                   // _logger.LogInformation(eventDetails.Description);
                    var user = _identityService.Get(HttpContext.User);
                    _logger.LogDebug("the event unit price is:" + eventDetails.Price);

                    var cartItem = new CartItem()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Quantity = 1,
                        EventName = eventDetails.Name,
                        PictureUrl = eventDetails.PictureUrl,
                        //need to add price property to event class
                        UnitPrice = eventDetails.Price,
                        EventId = eventDetails.Id
                    };
                   
                    await _cartService.AddItemToCart(user, cartItem);
                }
                return RedirectToAction("Index", "Event");
            }
            catch (BrokenCircuitException)
            {
                //Catch error when CartApi is in circuit-opened mode
               // HandleBrokenCircuitException();
            }
            return RedirectToAction("Index", "Event");            
        }
        private void HandleBrokenCircuitException()
        {
            TempData["BasketInoperativeMsg"] = "cart Service is inoperative, please try later on.(Business Msg Due to Circuit-Breaker) ";
        }
    }
}