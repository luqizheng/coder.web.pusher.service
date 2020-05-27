using System.Collections.Generic;

namespace Coder.WebPusherService.Senders.HttpSender
{
    public class HttpDictionaryContent : INotifyContent
    {
        public HttpDictionaryContent()
        {
            Content = new Dictionary<string, string>();
        }

        /// <summary>
        ///     用于body传递的数据
        /// </summary>
        public IDictionary<string, string> Content { get; set; }


        public string ResponseContent { get; set; }

        public int ResponseCode { get; set; }
    }
}