using System;
using System.Threading.Tasks;

namespace Coder.WebPusherService
{
    public abstract class NotifySettingBase
    {
        protected NotifySettingBase()
        {
        }

        protected NotifySettingBase(string messageType)
        {
            if (messageType == null) throw new ArgumentNullException(nameof(messageType));
            MessageType = messageType;
        }

        public int Id { get; set; }
        public string MessageType { get; set; }
        public int RetrySpreadSeconds { get; set; } = 20;
        public int MaxRetry { get; set; } = 3;

        public abstract Task<bool> Send(NotifyMessage message);
    }
}