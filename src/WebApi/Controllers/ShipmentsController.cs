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
        private readonly ICommandBus _commandBus;
        private readonly IShipmentDataService _shipmentDataService;

        public ShipmentsController(ICommandBus commandBus, IShipmentDataService shipmentDataService)
        {
            _commandBus = commandBus;
            _shipmentDataService = shipmentDataService;
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
        public void Post([FromBody]Addresses addresses)
        {
            var id = Guid.NewGuid().ToString();

            var command1 = new CreateShipment
            {
                Id = id,
                Address = addresses.InitialAddress
            };

            var command2 = new ChangeShipmentAddress
            {
                Id = id,
                NewAddress = addresses.NewAddress
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

    public class Addresses
    {
        public string InitialAddress { get; set; }
        public string NewAddress { get; set; }
    }
}
