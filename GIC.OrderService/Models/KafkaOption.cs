namespace GIC.OrderService.Models
{
    public class KafkaOption
    {
        public const string Key = "Kafka";
        public string BootstrapServers { get; set; }
    }
}
