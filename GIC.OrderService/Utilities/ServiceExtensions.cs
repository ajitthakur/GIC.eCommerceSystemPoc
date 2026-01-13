using GIC.OrderService.Data.Services;
using GIC.OrderService.Data.Services.Abstract;
using GIC.OrderService.Infrastructure;
using GIC.OrderService.Infrastructure.Abstract;
using GIC.UserService.Data.Services;
using System.Collections.Concurrent;

namespace GIC.OrderService.Utilities
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddKafkaEventPublishService(this IServiceCollection services)
        {            
            services.AddSingleton<IKafkaEventPublishService, KafkaEventPublishService>();
            return services;
        }

        public static IServiceCollection AddKafkaEventConsumeService(this IServiceCollection services)
        {
            services.AddSingleton<ConcurrentDictionary<Guid, (string, string)>>();
            services.AddHostedService<KafkaEventConsumeService>();
            return services;
        }

        public static IServiceCollection AddDataService(this IServiceCollection services)
        {
            services.AddScoped<IOrderDataService, OrderDataService>();
            return services;
        }
    }
}
