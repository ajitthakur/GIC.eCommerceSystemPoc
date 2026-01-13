using Confluent.Kafka;
using GIC.UserService.Infrastructure.Abstract;
using GIC.UserService.Models;
using GIC.UserService.Models.Events;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace GIC.UserService.Infrastructure
{
    public class KafkaEventConsumeService:  BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly KafkaOption _kafkaOption;
        private readonly ILogger<KafkaEventConsumeService> _logger;
        private readonly string consumeEventTopic = "Orders";
        private readonly ConcurrentDictionary<Guid, List<string>> _orderDataCache;
        public KafkaEventConsumeService(IOptions<KafkaOption> kafkaOption,
            ILogger<KafkaEventConsumeService> logger
            ,ConcurrentDictionary<Guid, List<string>> orderDataCache)
        {
            //_orderDataCache = orderDataCache;
            _kafkaOption = kafkaOption.Value;
            _logger = logger;

            var KafkaUrl = _kafkaOption.BootstrapServers    ;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = KafkaUrl,
                GroupId = "UserServiceConsumeGroup",
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
                var orderCreatedEvent = JsonConvert.DeserializeObject<OrderCreatedEvent>(message);
                _orderDataCache[orderCreatedEvent.UserId].Add(orderCreatedEvent.Product);
                Console.WriteLine(message);
                _logger.LogInformation($"Consume Order Event:{message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Consume order Event");
            }
        }
    }
}
