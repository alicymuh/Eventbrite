using EventbriteMvc.Models.CartModel;
using EventbriteMvc.Models.OrderModels;
using EventbriteMvc.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventbriteMvc.Services
{
   public interface ICartService
    {
        Task<Cart> GetCart(ApplicationUser user);
        Task AddItemToCart(ApplicationUser user, CartItem eventItem);
        Task<Cart> UpdateCart(Cart cart);
        Task<Cart> SetQuantities(ApplicationUser user, Dictionary<string, int> quantities);
         Order MapCartToOrder(Cart cart);
        Task ClearCart(ApplicationUser user);

        
    }
}
