using System;
using Coder.WebPusher.Stores;
using Coder.WebPusherService;
using Coder.WebPusherService.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Coder.WebPusher
{
    public static class LiteDbExtensions
    {
        public static NotifyServiceOptions AddNotifyServiceLiteDb(this NotifyServiceOptions builder, string folder)
        {
            builder.Services.AddScoped<INotifyMessageStore, NotifyMessageStore>(sp => new NotifyMessageStore(folder));
            builder.Services.AddScoped<INotifySettingStore, NotifySettingStore>(sp => new NotifySettingStore(folder));
            return builder;
        }
    }
}