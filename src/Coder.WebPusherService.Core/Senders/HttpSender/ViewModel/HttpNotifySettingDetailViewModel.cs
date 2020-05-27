using System;
using System.Collections.Generic;

namespace Coder.WebPusherService.Senders.HttpSender.ViewModel
{
    public class HttpNotifySettingDetailViewModel
    {
        public HttpNotifySettingDetailViewModel()
        {
        }
        public HttpNotifySettingDetailViewModel(HttpNotifySetting setting)
        {
            if (setting == null) throw new ArgumentNullException(nameof(setting));
            Id = setting.Id;
            Url = setting.Url;
            SendType = setting.SendType;
            Method = setting.Method;
            ContentType = setting.ContentType;
            SubmitDataTemplate = setting.SubmitDataTemplate;
            RawContentTemplate = setting.RawContentTemplate;
        }

        public int Id { get; set; }
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
    }
}