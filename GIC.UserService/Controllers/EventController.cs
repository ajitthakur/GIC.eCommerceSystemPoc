//using Confluent.Kafka;
//using Microsoft.AspNetCore.Mvc;

//namespace GIC.UserService.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class EventsController : ControllerBase
//    {
//        private readonly string _bootstrapServers;

//        public EventsController(IConfiguration config)
//        {
//            // Use localhost:9092 when running API on host
//            _bootstrapServers = config["Kafka:BootstrapServers"] ?? "localhost:9092";
//        }

//        [HttpPost]
//        public async Task<IActionResult> Publish([FromBody] string message)
//        {
//            try
//            {
//                var config = new ProducerConfig
//                {
//                    BootstrapServers = _bootstrapServers,
//                    MessageMaxBytes = 1048576,
//                };

//                using var producer = new ProducerBuilder<Null, string>(config).Build();
//                var dr = await producer.ProduceAsync("OrdersTest", new Message<Null, string> { Value = message });

//                return Ok($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
           
//        }
//    }

//}
