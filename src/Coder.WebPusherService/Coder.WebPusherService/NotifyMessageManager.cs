using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coder.WebPusherService.Queues;
using Coder.WebPusherService.Stores;

namespace Coder.WebPusherService
{
    public class NotifyMessageManager
    {
        private readonly INotifyMessageStore _notifyMessageStore;
        private readonly INotifySettingStore _notifySettingStore;

        private readonly NotifyQueue _queue;

        public NotifyMessageManager(INotifyMessageStore notifyMessageStore, INotifySettingStore notifySettingStore,
            NotifyQueue queue)
        {
            _notifyMessageStore = notifyMessageStore;
            _notifySettingStore = notifySettingStore;
            _queue = queue;
        }

        public Task Send(NotifyMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (string.IsNullOrWhiteSpace(message.MessageType))
                throw new ArgumentNullException(nameof(message), "message.messageType不能为空");
            var messageType = message.MessageType;
            var setting = _notifySettingStore.GetBy<NotifySettingBase>(messageType);

            message.StartSend();

            var task = setting.Send(message).ContinueWith(task =>
            {
                var successSend = task.Result;
                message.Sent(successSend);

                _notifyMessageStore.Update(message);
                _notifySettingStore.SaveChanged();

                if (!successSend && message.SendCount < setting.MaxRetry)
                    _queue.Add(message, DateTimeOffset.Now.AddSeconds(setting.RetrySpreadSeconds));
            });


            return task;
        }

        public NotifyMessage Retry(in int id)
        {
            var message = _notifyMessageStore.GetById(id);
            Send(message).Wait();
            return message;
        }

        public NotifyMessage GetById(int id)
        {
            return _notifyMessageStore.GetById(id);
        }

        public IEnumerable<NotifyMessage> FindByTag(string tag)
        {
            return _notifyMessageStore.FindByTag(tag);
        }
    }
}