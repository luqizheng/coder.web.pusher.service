using System.Net.Http;
using System.Text;

namespace Coder.WebPusherService.Senders.HttpSender.HttpNotify
{
    internal class HttpRawBodyContent : HttpSendContent
    {
        public string Content { get; set; }


        public override HttpContent MakeContent(string contentType)
        {
            return new StringContent(Content, Encoding.UTF8, contentType ?? "application/json");
        }

        public override string MakeQueryString()
        {
            return Content;
        }
    }
}