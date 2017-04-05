using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    using Contracts.Commands;
    using ParalectEventSourcing.Commands;

    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        private readonly ICommandBus _commandBus;

        public DevicesController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string shipmentKey)
        {
            var id = "a32b0aa8-ecc3-4de6-a323-72ca42f084b2";

            var command1 = new CreateShipment
            {
                Id = id,
                Address = "Minsk"
            };

            var command2 = new ChangeShipmentAddress
            {
                Id = id,
                NewAddress = "Den Haag"
            };

            _commandBus.Send(command1, command2);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
