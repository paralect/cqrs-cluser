using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    using Contracts.Commands;
    using DataService;
    using ParalectEventSourcing.Commands;

    [Route("api/[controller]")]
    public class ShipmentsController : Controller
    {
        private readonly IShipmentDataService _shipmentDataService;
        private readonly ICommandBus _commandBus;

        public ShipmentsController(IShipmentDataService shipmentDataService, ICommandBus commandBus)
        {
            _shipmentDataService = shipmentDataService;
            _commandBus = commandBus;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Shipment> Get()
        {
            return _shipmentDataService.GetAllShipments();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string address)
        {
            var command = new CreateShipment
            {
                Id = Guid.NewGuid().ToString(),
                Address = address,
                Metadata = new CommandMetadata
                {
                    CommandId = Guid.NewGuid().ToString(),
                    ConnectionId = Request.Headers["Connection-Id"],
                    ConnectionToken = Request.Headers["Connection-Token"]
                }
            };

            _commandBus.Send(command);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]string newAddress)
        {
            var command = new ChangeShipmentAddress
            {
                Id = id,
                NewAddress = newAddress,
                Metadata = new CommandMetadata
                {
                    CommandId = Guid.NewGuid().ToString(),
                    ConnectionId = Request.Headers["Connection-Id"]
                }
            };

            _commandBus.Send(command);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
