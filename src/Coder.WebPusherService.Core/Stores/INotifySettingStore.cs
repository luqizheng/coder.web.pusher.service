using System.Collections.Generic;

namespace Coder.WebPusherService.Stores
{
    public interface INotifySettingStore
    {
        T GetById<T>(int id) where T : NotifySettingBase;
        T GetBy<T>(string messageType) where T : NotifySettingBase;
        void SaveOrUpdate(NotifySettingBase setting);
        void SaveChanged();
        void DeleteById(in int id);
        IEnumerable<T> List<T>(string messageType, in int page, in int pageSize,out int total) where T : NotifySettingBase;
    }
}