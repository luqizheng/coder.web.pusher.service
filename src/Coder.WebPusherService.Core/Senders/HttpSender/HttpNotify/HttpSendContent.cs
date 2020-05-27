using System.Net.Http;

namespace Coder.WebPusherService.Senders.HttpSender.HttpNotify
{
    internal abstract class HttpSendContent
    {
        public string Url { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseContent { get; set; }

        public abstract HttpContent MakeContent(string contentType);

        public abstract string MakeQueryString();
    }
}