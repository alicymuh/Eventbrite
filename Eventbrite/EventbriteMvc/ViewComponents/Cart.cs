using EventbriteMvc.Models.UserModel;
using EventbriteMvc.Services;
using EventbriteMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventbriteMvc.ViewComponents
{
    public class Cart : ViewComponent
    {
        private readonly ICartService _cartService;

        public Cart(ICartService cartService) => _cartService = cartService;
        public async Task<IViewComponentResult> InvokeAsync(ApplicationUser user)
        {
            var vm = new CartComponentViewModel();
            try
            {
                var cart = await _cartService.GetCart(user);
                vm.ItemsInCart = cart.Events.Count;
                vm.TotalCost = cart.Total();
                return View(vm);
            }
            catch (BrokenCircuitException)
            {
                //Catch error when CartApi is in open circuit mode
                ViewBag.IsBasketInoperative = true;
            }
            return View(vm);
        }

    }
}
