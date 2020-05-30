using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Coder.WebPusherService.Senders.HttpSender;
using Coder.WebPusherService.Senders.HttpSender.ViewModel;
using Coder.WebPusherService.ViewModels;
using Newtonsoft.Json;

namespace Coder.WebPusherClient
{
    public class HttpMessageSettingClient
    {
        private const string Path = "/pusher-service/manage/HttpNotifySetting";
        private readonly string _url;
        private HttpClient _httpClient;

        public HttpMessageSettingClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public HttpMessageSettingClient(string url)
        {
            _url = url;
        }

        private HttpClient GetClient()
        {
            if (_httpClient == null) return _httpClient = new HttpClient { BaseAddress = new Uri(_url) };

            return _httpClient;
        }

        public HttpNotifySettingDetailViewModel Get(int id)
        {
            var response = _httpClient.GetAsync(Path + "/" + id).Result;

            var str = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<HttpNotifySettingDetailViewModel>(str);
            return result;
        }

        public NotifyResult Save(HttpNotifySettingDetailViewModel settring)
        {
            var str = JsonConvert.SerializeObject(settring);
            var content = new StringContent(str, Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync(Path + "/save", content).Result;
            var responseStr = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<NotifyResult>(responseStr);
            return result;
        }
    }

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