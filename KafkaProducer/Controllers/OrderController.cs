using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaProducer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KafkaProducer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ProducerConfig _config;

        public OrderController(ProducerConfig config)
        {
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(OrderRequest value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string serializedOrder = JsonConvert.SerializeObject(value);

            var config = new ProducerConfig{
                BootstrapServers = "localhost:9092"
            };

            var producer = new ProducerWrapper(config, "orderrequests");

            await producer.WriteMessage(serializedOrder);

            return Created("TransactionId", "Your order is in progress");
        }
    }
}
