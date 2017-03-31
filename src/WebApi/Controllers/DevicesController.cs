using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    using System.Text;
    using Contracts.Commands;
    using Newtonsoft.Json;
    using ParalectEventSourcing.Commands;
    using RabbitMQ.Client;

    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        private const string WriteModelQueue = "WriteModelQueue";
        private const string ErrorQueue = "ErrorQueue";

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
            var command = new AddDeviceToShipmentCommand
            {
                Id = "2d810a20-490e-4fee-845d-5a7f5de0fc42",
                ShipmentKey = shipmentKey,
                Metadata = new CommandMetadata
                {
                    CommandId = Guid.NewGuid().ToString(),
                    CreatedDate = DateTime.UtcNow,
                    TypeName = typeof(AddDeviceToShipmentCommand).AssemblyQualifiedName,
                    UserId = "123"
                }
            };

            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: WriteModelQueue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var data = JsonConvert.SerializeObject(command);
                var body = Encoding.UTF8.GetBytes(data);

                channel.BasicPublish(exchange: "",
                                 routingKey: WriteModelQueue,
                                 basicProperties: null,
                                 body: body);
                Console.WriteLine(" [x] Sent {0}", JsonConvert.SerializeObject(command));
            }
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
