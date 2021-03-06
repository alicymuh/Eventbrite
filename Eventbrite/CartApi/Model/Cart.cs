﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Model
{
    public class Cart
    {
        public string BuyerId { get; set; }
        public List<CartEvent> Events { get; set; }

        public Cart(string cartId)
        {
            BuyerId = cartId;
            Events = new List<CartEvent>();
        }
    }
}
