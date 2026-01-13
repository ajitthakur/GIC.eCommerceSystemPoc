using GIC.UserService.Data.Services;
using GIC.UserService.Data.Services.Abstract;
using GIC.UserService.Infrastructure;
using GIC.UserService.Infrastructure.Abstract;
using GIC.UserService.Models.Events;
using System.Collections.Concurrent;

namespace GIC.UserService.Utilities
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
            services.AddSingleton<ConcurrentDictionary<Guid, List<string>>>();
            services.AddHostedService<KafkaEventConsumeService>();
            return services;
        }

        public static IServiceCollection AddDataService(this IServiceCollection services)
        {
            services.AddScoped<IUserDataService, UserDataService>();
            return services;
        }
    }
}
