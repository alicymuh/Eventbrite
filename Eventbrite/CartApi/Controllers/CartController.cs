using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CartApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CartApi.Controllers
{
    [Route("api/v1/[controller]")]

    public class CartController : Controller
    {
        private ICartRepository _repository;
        private ILogger _logger;
        public CartController(ICartRepository repository, ILoggerFactory factory)
        {
            _repository = repository;
            _logger = factory.CreateLogger<CartController>();
        }

        //Get api/values/3
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Model.Cart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string id)
        {
            var basket = await _repository.GetCartAsync(id);
            return Ok(basket);
        }

        //POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(Model.Cart), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> Post([FromBody] Model.Cart value)
        {
            var basket = await _repository.UpdateCartAsync(value);
            return Ok(basket);

            /*var testbasket = await _repository.CheckPersistency(value);
            return Ok(testbasket);*/
        }
       
        //DELETE api/values/5
        [HttpDelete("{id}")]

        public void Delete(string id)
        {
            _logger.LogInformation("Delete method in Cart controller reached");
            _repository.DeleteCartAsync(id);
        }
    }
}