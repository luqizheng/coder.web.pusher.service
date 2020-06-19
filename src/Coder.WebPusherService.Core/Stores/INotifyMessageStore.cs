using System.Collections.Generic;
using Coder.WebPusherService;

namespace Coder.WebPusherService.Stores
{
   public interface INotifyMessageStore
    {
        void Update(NotifyMessage message);
        void SaveChange();
        NotifyMessage GetById(int id);
        IEnumerable<NotifyMessage> FindByTag(string tag);
        IEnumerable<NotifyMessage> GetUnsentMessage(string settingMessageType);
    }
}
