using Microsoft.Extensions.DependencyInjection;

namespace Coder.WebPusherService
{
    public class NotifyServiceOptions
    {
        public NotifyServiceOptions(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}