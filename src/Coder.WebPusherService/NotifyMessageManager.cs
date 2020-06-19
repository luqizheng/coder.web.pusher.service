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
            if (setting == null)
                throw new NotifyMessageException("找不到类型是：" + messageType + "的设置");
            if (setting.Status == NotifySettingStatus.Normal)
            {
                message.StartSend();

                var task = setting.Send(message).ContinueWith(task =>
                {
                    var successSend = task.Result;
                    message.Sent(successSend);

                    _notifyMessageStore.Update(message);
                    _notifySettingStore.SaveChanged();

                    if (!successSend)
                    {
                        if (message.SendCount <= setting.MaxRetry + 1)
                            _queue.Add(message, DateTimeOffset.Now.AddSeconds(setting.RetrySpreadSeconds));
                        else
                            message.FailToSent();
                    }
                });


                return task;
            }

            if (setting.Status == NotifySettingStatus.Pause)
            {
                _notifyMessageStore.Update(message);
                _notifySettingStore.SaveChanged();
            }

            return Task.FromResult(0);
        }

        public void Resume(int id)
        {
            var setting = _notifySettingStore.GetById<NotifySettingBase>(id);
            if (setting == null)
                throw new NotifyMessageException("找不到id是：" + id + "的设置");


            var messages = _notifyMessageStore.GetUnsentMessage(setting.MessageType);
            foreach (var message in messages) Send(message);

            setting.Resume();
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

        public void ResumeAll()
        {
            foreach (var setting in _notifySettingStore.GetAll<NotifySettingBase>())
            {
                setting.Pause();
                Resume(setting.Id);
            }
        }
    }
}