using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Coder.WebPusherService.Senders.HttpSender.HttpNotify;

namespace Coder.WebPusherService.Senders.HttpSender
{
    public enum SendType
    {
        FormUrlEncoding,
        Raw
    }

    public class HttpNotifySetting : NotifySettingBase
    {
        public static Regex ValMatch = new Regex("\\{(.+?)\\}");

        public HttpNotifySetting()
        {

        }

        public HttpNotifySetting(string messageType) : base(messageType)
        {

        }
        public string Url { get; set; }
        public HttpNotifyMessageMethod Method { get; set; }
        public SendType SendType { get; set; }

        /// <summary>
        ///     contentType，默认null的情况下， 如果SendType就用Application/Json,否则就是formUrlEncoding,
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     当SendType=FormUrlEncoding的时候使用这个template。
        /// </summary>
        public IList<Variable> SubmitDataTemplate { get; set; } = new List<Variable>();

        /// <summary>
        ///     当SendType=Raw的时候使用这个template
        /// </summary>
        public string RawContentTemplate { get; set; }

        public string FormatUrl(IDictionary<string, string> urlData)
        {
            if (urlData == null || urlData.Count == 0)
                return Url;

            return ValMatch.Replace(Url, s => urlData[s.Groups[1].Value]);
        }

        private IDictionary<string, string> Mix(IDictionary<string, string> content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (SubmitDataTemplate.Count == 0) return content;

            foreach (var key in SubmitDataTemplate)
                if (!content.ContainsKey(key.Name))
                    content.Add(key.Name, key.Value);


            return content;
        }


        public override Task<bool> Send(NotifyMessage message)
        {
            var httpSender = new HttpClientSender();
            HttpSendContent httpSendContent = null;
            var httpContent = (HttpDictionaryContent)message.Content;
            switch (SendType)
            {
                case SendType.FormUrlEncoding:
                    httpSendContent = MakeRawContent(httpContent);
                    break;
                case SendType.Raw:
                    httpSendContent = MakeRawContent(httpContent);
                    break;
            }

            return Task.Run(() =>
            {
                var result = httpSender.Send(httpSendContent, this);
                httpContent.ResponseCode = httpSendContent.ResponseCode;
                httpContent.ResponseContent = httpSendContent.ResponseContent;
                return result;
            });
        }

        private HttpFormSendContent MakeDictContent(HttpDictionaryContent dictionaryContent)
        {
            var dict = Mix(dictionaryContent.Content);
            var url = FormatUrl(dictionaryContent.Content);

            return new HttpFormSendContent
            {
                Content = dict,
                Url = url
            };
        }

        private HttpRawBodyContent MakeRawContent(HttpDictionaryContent dictionaryContent)
        {
            var dict = Mix(dictionaryContent.Content);
            var rawContent = ValMatch.Replace(RawContentTemplate, s => dict[s.Groups[1].Value]);
            var url = FormatUrl(dictionaryContent.Content);
            return new HttpRawBodyContent
            {
                Content = rawContent,
                Url = url
            };
        }
    }
}