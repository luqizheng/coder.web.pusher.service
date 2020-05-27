using System;
using System.IO;
using Coder.WebPusherService;
using Coder.WebPusherService.Stores;
using LiteDB;

namespace Coder.WebPusher.Stores
{
    public class NotifySettingStore : INotifySettingStore
    {
        private readonly string _folder;

        public NotifySettingStore(string folder)
        {
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            _folder = folder;
        }

        public T GetById<T>(int id) where T : NotifySettingBase
        {
            var dbFolder = Path.Combine(_folder, "notifySettings.db");
            using (var db = new LiteDatabase(dbFolder))
            {
                // Get customer collection
                var customers = db.GetCollection<T>("notifySetting");
                customers.EnsureIndex(_ => _.MessageType, true);
                return customers.FindOne(_ => _.Id == id);
            }
        }

        public T GetBy<T>(string messageType) where T : NotifySettingBase
        {
            if (messageType == null) throw new ArgumentNullException(nameof(messageType));
            var dbFolder = Path.Combine(_folder, "notifySettings.db");
            using (var db = new LiteDatabase(dbFolder))
            {
                // Get customer collection
                var customers = db.GetCollection<T>("notifySetting");
                customers.EnsureIndex(_ => _.MessageType, true);
                return customers.FindOne(_ => _.MessageType == messageType);
            }
        }

        public void SaveChanged()
        {
        }

        public void DeleteById(in int id)
        {
            var dbFolder = Path.Combine(_folder, "notifySettings.db");
            using (var db = new LiteDatabase(dbFolder))
            {
                // Get customer collection
                var customers = db.GetCollection<NotifySettingBase>("notifySetting");
                customers.EnsureIndex(_ => _.MessageType, true);
                customers.Delete(id);
            }
        }

        public void SaveOrUpdate(NotifySettingBase setting)
        {
            if (setting == null) throw new ArgumentNullException(nameof(setting));
            var dbFolder = Path.Combine(_folder, "notifySettings.db");
            using (var db = new LiteDatabase(dbFolder))
            {
                // Get customer collection
                var customers = db.GetCollection<NotifySettingBase>("notifySetting");
                customers.EnsureIndex(_ => _.MessageType, true);

                var exist = customers.FindOne(_ => _.MessageType == setting.MessageType);
                if (exist != null)
                {
                    throw new ArgumentNullException(nameof(setting), setting.MessageType + "已经存在相同的类型。");
                }

                if (setting.Id == 0)
                    customers.Insert(setting);
                customers.Update(setting);
            }
        }
    }
}