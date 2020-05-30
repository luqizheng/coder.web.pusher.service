using Coder.WebPusher.Stores;
using Coder.WebPusherService;
using Coder.WebPusherService.Senders.HttpSender;
using Xunit;

namespace XUnitTestProject1.Litdb
{
    public class HttpNotifySettingTest
    {
        [Fact]
        public void Test()
        {
            var store = new NotifySettingStore("dbFolder");
            var actual = new HttpNotifySetting
            {
                ContentType = "test"
            };
            store.SaveOrUpdate(actual);
            Assert.NotEqual(0, actual.Id);


            var target = store.GetBy<NotifySettingBase>(actual.ContentType);
            Assert.NotNull(target);
        }
    }
}