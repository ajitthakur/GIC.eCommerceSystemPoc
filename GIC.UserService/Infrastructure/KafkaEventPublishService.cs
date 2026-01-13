using Confluent.Kafka;
using GIC.UserService.Infrastructure.Abstract;
using GIC.UserService.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;


namespace GIC.UserService.Infrastructure
{
    public class KafkaEventPublishService : IKafkaEventPublishService
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaOption _kafkaOption;
        private readonly ILogger<KafkaEventPublishService> _logger;

        public KafkaEventPublishService(IOptions<KafkaOption> kafkaOption,
            ILogger<KafkaEventPublishService> logger) 
        {
            _kafkaOption = kafkaOption.Value;
            _logger = logger;            
            _producer = new ProducerBuilder<string, string>(new ProducerConfig
            {
                BootstrapServers = _kafkaOption.BootstrapServers,
                MessageMaxBytes = 500000000,   
                CompressionType = CompressionType.Snappy,
                //MessageSendMaxRetries = 3,
                //LingerMs = 5 // delay
            }).Build();
        }

        public async Task Publish<T>(string key, T requestObject, CancellationToken ct = default, string eventTopic="Users")
        {
            try
            {
                var msg = new Message<string, string>
                {
                    Key = key, // User Id
                    Value = JsonSerializer.Serialize(requestObject) //new string('x', 1024 * 1024 * 10) //
                };

                 var deliveryResult = await _producer.ProduceAsync(eventTopic, msg);
                _logger.LogInformation($"Order Service - Publish {eventTopic} , {deliveryResult.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> ex)
            {
                _logger.LogError($"Order Service Publish failed: {ex.Error.Reason}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Publish failed");
            }           
        }
    }
}
