﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Model
{
   public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string cartId);
        IEnumerable<string> GetUsers();
        Task<Cart> UpdateCartAsync(Cart basket);
        Task<bool> DeleteCartAsync(string id);

        Task<Cart> CheckPersistency(Cart basket);
    }
}
