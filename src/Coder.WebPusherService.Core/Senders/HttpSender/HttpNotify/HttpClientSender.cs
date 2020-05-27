using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Coder.WebPusherService.Senders.HttpSender.HttpNotify
{
    internal class HttpClientSender
    {
        public bool Send(HttpSendContent message, HttpNotifySetting setting)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (setting == null) throw new ArgumentNullException(nameof(setting));
            try
            {
                return SentToHttp(message, setting);
            }
            catch (Exception ex)
            {
                message.ResponseContent = ex.Message;
                message.ResponseContent += "\r\n" + ex.StackTrace;
                return false;
            }
        }


        private bool SentToHttp(HttpSendContent message, HttpNotifySetting setting)
        {
            var httpClient = new HttpClient();


            Task<HttpResponseMessage> responseTask = null;
            switch (setting.Method)
            {
                case HttpNotifyMessageMethod.GET:
                    var queryString = message.MakeQueryString();
                    responseTask = httpClient.GetAsync(message.Url + "?" + queryString);
                    break;
                case HttpNotifyMessageMethod.POST:
                    responseTask = httpClient.PostAsync(message.Url, message.MakeContent(setting.ContentType));
                    break;
                case HttpNotifyMessageMethod.PUT:
                    responseTask = httpClient.PutAsync(message.Url, message.MakeContent(setting.ContentType));
                    break;
            }

            if (responseTask != null)
            {
                message.ResponseCode = (int) responseTask.Result.StatusCode;
                message.ResponseContent = responseTask.Result.Content.ReadAsStringAsync().Result;
            }

            return responseTask != null && responseTask.Result.IsSuccessStatusCode;
        }
    }
}