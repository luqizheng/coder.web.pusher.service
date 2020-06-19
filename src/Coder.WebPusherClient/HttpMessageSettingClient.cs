using Coder.WebPusherService.Senders.HttpSender.ViewModel;
using Coder.WebPusherService.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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

        public NotifyResult Save(HttpNotifySettingDetailViewModel setting)
        {
            var str = JsonConvert.SerializeObject(setting);
            var content = new StringContent(str, Encoding.UTF8, "application/json");
            var response = GetClient().PostAsync(Path + "/save", content).Result;
            var responseStr = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<NotifyResult>(responseStr);
                return result;
            }
            throw new Exception("Exception from server:" + responseStr);
        }

        public IEnumerable<HttpNotifySettingDetailViewModel> List(out int total, string messageType = null, int page = 1, int pageSize = 30)
        {
            var queryString = $"page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(messageType)) queryString += "&messageType=" + messageType;

            var response = GetClient().GetAsync(Path + "/list?" + queryString).Result;
            var str = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<HttpNotifySettingDetailViewModelResult>(str);
            total = result.Count;
            return result.Data;
        }

        private class HttpNotifySettingDetailViewModelResult
        {
            public IEnumerable<HttpNotifySettingDetailViewModel> Data { get; set; }
            public int Count { get; set; }
        }
    }
}
