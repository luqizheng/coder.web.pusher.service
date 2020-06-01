using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Coder.WebPusherService;
using Coder.WebPusherService.Stores;
using LiteDB;

namespace Coder.WebPusher.Stores
{


    internal class NotifyMessageStore : INotifyMessageStore
    {
        private readonly string _folder;

        public NotifyMessageStore(string folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));
            FileUtility.AutoCreateDirectory(folder);
            _folder = folder;
        }

        public void Update(NotifyMessage message)
        {
            var dbFolder = Path.Combine(_folder, "message.db");

            using (var db = new LiteDatabase(dbFolder))
            {
                // Get customer collection
                var customers = db.GetCollection<NotifyMessage>("notifyId");

                if (message.Id == 0)
                    customers.Insert(message);
                else
                    customers.Update(message);
            }
        }

        public void SaveChange()
        {

        }

        public NotifyMessage GetById(int id)
        {
            var dbFolder = Path.Combine(_folder, "message.db");
            using (var db = new LiteDatabase(dbFolder))
            {
                // Get customer collection
                var customers = db.GetCollection<NotifyMessage>("notifySetting");

                return customers.FindOne(_ => _.Id == id);
            }
        }


        public IEnumerable<NotifyMessage> FindByTag(string tag)
        {

            var dbFolder = Path.Combine(_folder, "message.db");
            using (var db = new LiteDatabase(dbFolder))
            {
                // Get customer collection
                var customers = db.GetCollection<NotifyMessage>("notifySetting");

                return customers.Find(_ => _.Tag == tag).ToList();
            }
        }
    }
}