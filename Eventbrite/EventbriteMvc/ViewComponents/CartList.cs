using EventbriteMvc.Models.UserModel;
using EventbriteMvc.Models.CartModel;
using EventbriteMvc.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly.CircuitBreaker;

namespace EventbriteMvc.ViewComponents
{
    public class CartList:ViewComponent
    {
        private readonly ICartService _cartService;

        public CartList(ICartService cartService) => _cartService = cartService;
        public async Task<IViewComponentResult> InvokeAsync(ApplicationUser user)
        {
            var vm = new Models.CartModel.Cart();
            try
            {
                vm = await _cartService.GetCart(user);
                return View(vm);
            }
            catch (BrokenCircuitException)
            {
                // Catch error when CartApi is in open circuit mode
                ViewBag.IsBasketInoperative = true;
                TempData["BasketInoperativeMsg"] = "Basket Service is inoperative, pleasr try later on.(Business Mag due to Circuit-Breaker)";
            }
            return View(vm);
        }
    }
}
