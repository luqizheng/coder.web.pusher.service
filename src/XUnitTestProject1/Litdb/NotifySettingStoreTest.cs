using Coder.WebPusher.Stores;
using Coder.WebPusherService;
using Coder.WebPusherService.Senders.HttpSender;
using System.IO;
using Xunit;

namespace XUnitTestProject1.Litdb
{
    public class NotifySettingStoreTest
    {
        [Fact]
        public void Test()
        {
            if (Directory.Exists("dbFolder"))
            {
                Directory.Delete("dbFolder", true
                );
            }
            var store = new NotifySettingStore("dbFolder");
            var actual = new HttpNotifySetting
            {
                SendContentType = "test",
                MessageType = "justTest",
            };
            store.SaveOrUpdate(actual);
            Assert.NotEqual(0, actual.Id);


            var target = store.GetBy<NotifySettingBase>(actual.MessageType);
            Assert.NotNull(target);
        }
    }
}
