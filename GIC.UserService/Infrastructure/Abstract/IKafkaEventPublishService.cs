namespace GIC.UserService.Infrastructure.Abstract
{
    public interface IKafkaEventPublishService
    {
        Task Publish<T>(string key, T requestObject, CancellationToken ct = default, string eventTopic = "Users");
    }
}
