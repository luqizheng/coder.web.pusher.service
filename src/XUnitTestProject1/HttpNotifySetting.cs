using System.Collections.Generic;
using Coder.WebPusherService.Senders.HttpSender;
using Xunit;

namespace XUnitTestProject1
{
    public class HttpNotifySettingTest
    {
        [Fact]
        public void Test1_URL()
        {
            var sett = new HttpNotifySetting();
            sett.Url = "http://10.33.22.11/{code}/{merchant}/jkkks";
            var urlData = new Dictionary<string, string>();
            urlData.Add("code", "a4-123");
            urlData.Add("merchant", "merchantA-2");
            var actual = sett.FormatUrl(urlData);

            var url = sett.Url;
            foreach (var key in urlData.Keys) url = url.Replace("{" + key + "}", urlData[key]);
            Assert.Equal(url, actual);
        }


        [Fact]
        public void Test2_URL()
        {
            var sett = new HttpNotifySetting();
            sett.Url = "http://10.33.22.11/jkkks";
            var urlData = new Dictionary<string, string>();
            urlData.Add("code", "a4-123");
            urlData.Add("merchant", "merchantA-2");
            var actual = sett.FormatUrl(urlData);

            var url = sett.Url;
         
            Assert.Equal(url, actual);
        }

        [Fact]
        public void Test4_()
        {
            var sett = new HttpNotifySetting();
            sett.Url = "http://10.33.22.11/jkkks";
            var urlData = new Dictionary<string, string>();
            urlData.Add("code", "a4-123");
            urlData.Add("merchant", "merchantA-2");
            var actual = sett.FormatUrl(urlData);

            var url = sett.Url;

            Assert.Equal(url, actual);
        }
    }
}