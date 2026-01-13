using Confluent.Kafka;
using GIC.OrderService.Infrastructure.Abstract;
using GIC.OrderService.Models;
using GIC.OrderService.Models.Events;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace GIC.OrderService.Infrastructure
{
    public class KafkaEventConsumeService:  BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly KafkaOption _kafkaOption;
        private readonly ILogger<KafkaEventConsumeService> _logger;
        private readonly string consumeEventTopic = "Users";
        private readonly ConcurrentDictionary<Guid, (string, string)> _userDataCache;

        public KafkaEventConsumeService(IOptions<KafkaOption> kafkaOption,
            ConcurrentDictionary<Guid, (string, string)> userDataCache,
            ILogger<KafkaEventConsumeService> logger)
        {
            _kafkaOption = kafkaOption.Value;
            _userDataCache = userDataCache;
            _logger = logger;

            var KafkaUrl = _kafkaOption.BootstrapServers    ;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = KafkaUrl,
                GroupId = "OrderServiceConsumeGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            _consumer.Subscribe(consumeEventTopic);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() => ProcessMessageObject(stoppingToken), stoppingToken);
                await Task.Delay(10, stoppingToken);
            }

            _consumer.Close();
        }

        public void ProcessMessageObject(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                var userCreatedEvent = JsonConvert.DeserializeObject<UserCreatedEvent>(message);
                _userDataCache[userCreatedEvent.Id] = (userCreatedEvent.Name, userCreatedEvent.Email);
                Console.WriteLine(message);
                _logger.LogInformation($"Consume User Event:{message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Consume User Event");
            }
        }
    }
}
