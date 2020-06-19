using Coder.WebPusherService;
using Coder.WebPusherService.Stores;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public IEnumerable<T> List<T>(string messageType, in int page, in int pageSize, out int total)
            where T : NotifySettingBase
        {
            var dbFolder = Path.Combine(_folder, "notifySettings.db");
            using var db = new LiteDatabase(dbFolder);
            // Get customer collection
            var settings = db.GetCollection<T>("notifySetting");
            settings.EnsureIndex(_ => _.MessageType, true);
            var skip = (page - 1) * pageSize;

            if (!string.IsNullOrWhiteSpace(messageType))
            {
                total = settings.Count(_ => _.MessageType == messageType);
                return settings.Find(_ => _.MessageType == messageType, skip, pageSize).ToList();
            }

            total = settings.Count();
            return settings.Find(_ => true, skip, pageSize).ToList();
        }

        public IEnumerable<T> GetAll<T>() where T : NotifySettingBase
        {
            var dbFolder = Path.Combine(_folder, "notifySettings.db");
            using var db = new LiteDatabase(dbFolder);
            // Get customer collection
            var settings = db.GetCollection<T>("notifySetting");
            settings.EnsureIndex(_ => _.MessageType, true);
            return settings.FindAll().ToList();
        }

        public void SaveOrUpdate(NotifySettingBase setting)
        {
            if (setting == null) throw new ArgumentNullException(nameof(setting));
            if (setting.MessageType == null) throw new NotifySettingException("请输入MessageType");
            var dbFolder = Path.Combine(_folder, "notifySettings.db");
            using var db = new LiteDatabase(dbFolder);
            // Get customer collection
            var customers = db.GetCollection<NotifySettingBase>("notifySetting");
            customers.EnsureIndex(_ => _.MessageType, true);


            if (setting.Id == 0)
            {
                var exist = customers.FindOne(_ => _.MessageType == setting.MessageType);
                if (exist != null)
                    throw new NotifySettingException(setting.MessageType + "已经存在相同的类型" + setting.MessageType);
                var id = customers.Insert(setting);
                setting.Id = id.AsInt32;
            }
            else
            {
                customers.Update(setting);
            }
        }
    }
}
