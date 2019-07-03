﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderApi.Data;
using OrderApi.Messaging;
using OrderApi.Models;

namespace OrderApi.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly OrdersContext _ordersContext;
        private readonly IOptionsSnapshot<OrderSettings> _settings;
        private readonly ILogger<OrdersController> _logger;
        private IBus _bus;


        public OrdersController(OrdersContext ordersContext, ILogger<OrdersController>logger, IOptionsSnapshot<OrderSettings> settings, IBus bus)
        {
            _settings = settings;
            _ordersContext = ordersContext ?? throw new ArgumentNullException(nameof(ordersContext));
            ((DbContext)ordersContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _bus = bus;
            _logger = logger;

        }

       /* // POST api/Order
        [Route("new")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> CreateOrder()
        {
            return Ok("Now it's time to convert a basket into an order");
        }*/

        // POST api/Order
        [Route("new")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> CreateOrder ([FromBody] Order order)
        {
            var envs = Environment.GetEnvironmentVariables();
            var conString = _settings.Value.ConnectionString;
            _logger.LogInformation($"{conString}");

            order.OrderStatus = OrderStatus.Preparing;
            order.orderDate = DateTime.UtcNow;

            _logger.LogInformation("Create Order started");
            _logger.LogInformation("Order" + order.UserName);

            _ordersContext.Orders.Add(order);
            _ordersContext.OrderItems.AddRange(order.OrderItems);

            _logger.LogInformation("Order added to context");
            _logger.LogInformation("Saving....");
            try
            {
                await _ordersContext.SaveChangesAsync();
                _logger.LogWarning("BuyerId is:" + order.BuyerId);
                await _bus.Publish(new OrderCompletedEvent(order.BuyerId));
                return CreatedAtRoute("GetOrder", new { id = order.OrderId }, order);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("An error occured during Order saving.." + ex.Message);
                return BadRequest();
            }

        }

        [HttpGet("{id}", Name = "GetOrder")]
        //  [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> GetOrder(int id)
        {
            var item = await _ordersContext.Orders
                .Include(x => x.OrderItems)
                .SingleOrDefaultAsync(ci => ci.OrderId == id);
            if (item != null)
            {
                return Ok(item);
            }
            return Ok("The Order was not placed");
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> GetOrders()
        {
            var orders = await _ordersContext.Orders.ToListAsync();
            return Ok(orders);
        }

    }
}