using System;
using Coder.WebPusherService.Queues;
using Microsoft.Extensions.DependencyInjection;

namespace Coder.WebPusherService
{
    public static class NotifyExtensions
    {
        public static IServiceCollection AddNotifyService(this IServiceCollection services,
            Action<NotifyServiceOptions> buildAction)
        {
            var result = new NotifyServiceOptions(services);
            services.AddSingleton<NotifyQueue>().AddSingleton<NotifyQueueService>();
            buildAction(result);
            return services;
        }
    }
}