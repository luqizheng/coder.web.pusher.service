using Coder.WebPusherService.Senders.HttpSender;
using Coder.WebPusherService.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Coder.WebPusherClient
{
    public class HttpPusherClient
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _host;

        public HttpPusherClient(string host)
        {
            if (!host.EndsWith("/"))
                host += "/";
            _host = host;
        }

        public NotifyMessageViewModel<HttpDictionaryContent> Send(HttpDictionaryContent content, string messageType,
            string tag)
        {
            if (messageType == null) throw new ArgumentNullException(nameof(messageType));
            var data = new
            {
                content,
                tag
            };
            var json = JsonConvert.SerializeObject(data);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var task = _client.PostAsync(_host + "HttpNotifyMessage/send" + messageType, stringContent);


            var jsonResult = task.Result.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<NotifyMessageViewModel<HttpDictionaryContent>>(jsonResult);
            return result;
        }

        public NotifyMessageViewModel<HttpDictionaryContent> Retry(string id)
        {
            var task = _client.GetAsync(_host + "HttpNotifyMessage/retry/" + id);
            var jsonResult = task.Result.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<NotifyMessageViewModel<HttpDictionaryContent>>(jsonResult);
            return result;
        }

        public NotifyMessageViewModel<HttpDictionaryContent> Get(string id)
        {
            var task = _client.GetAsync(_host + "HttpNotifyMessage/get/" + id);
            var jsonResult = task.Result.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<NotifyMessageViewModel<HttpDictionaryContent>>(jsonResult);
            return result;
        }

        public IEnumerable<NotifyMessageViewModel<HttpDictionaryContent>> List(string tag)
        {
            var task = _client.GetAsync(_host + "HttpNotifyMessage/list/?tag=" + tag);
            var jsonResult = task.Result.Content.ReadAsStringAsync().Result;
            var result =
                JsonConvert.DeserializeObject<IEnumerable<NotifyMessageViewModel<HttpDictionaryContent>>>(jsonResult);
            return result;
        }
    }
}
