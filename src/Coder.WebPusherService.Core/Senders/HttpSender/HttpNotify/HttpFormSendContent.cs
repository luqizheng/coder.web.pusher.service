using System.Collections.Generic;
using System.Net.Http;

namespace Coder.WebPusherService.Senders.HttpSender.HttpNotify
{
    internal class HttpFormSendContent : HttpSendContent
    {
        public IDictionary<string, string> Content { get; set; }

        public override HttpContent MakeContent(string contentType)
        {
            return new FormUrlEncodedContent(Content);
        }

        public override string MakeQueryString()
        {
            return MakeQueryString(Content);
        }

        public string MakeQueryString(IDictionary<string, string> urlQueryString)
        {
            var list = new List<string>();
            foreach (var paire in urlQueryString) list.Add(paire.Key + "=" + paire.Value);

            return string.Join("&", list.ToArray());
        }
    }
}