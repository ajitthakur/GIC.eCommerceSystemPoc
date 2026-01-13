using Confluent.Kafka;
using GIC.OrderService.Infrastructure.Abstract;
using GIC.OrderService.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GIC.OrderService.Infrastructure
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

        public async Task Publish<T>(string key, T requestObject, CancellationToken ct = default, string eventTopic= "Orders")
        {
            try
            {
                var msg = new Message<string, string>
                {
                    Key = key, // Order Id
                    Value = JsonSerializer.Serialize(requestObject)
                };

                var dr = await _producer.ProduceAsync(eventTopic, msg, ct);
                _logger.LogInformation($"Order Service - Publish {eventTopic} , {dr.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> ex)
            {
                _logger.LogError($"Order Service  Publish failed: {ex.Error.Reason}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Order Service Publish failed");
            }           
        }
    }
}
