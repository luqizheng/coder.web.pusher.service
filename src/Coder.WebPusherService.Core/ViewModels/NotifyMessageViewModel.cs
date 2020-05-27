using System;

namespace Coder.WebPusherService.ViewModels
{
    public class NotifyMessageViewModel<T> where T : INotifyContent
    {
    

        public NotifyMessageViewModel(NotifyMessage message)
        {
        
            Id = message.Id;
            Tag = message.Tag;
            Status = message.Status;
            CompleteSendTime = message.CompleteSendTime;
            StartSendTime = message.StartSendTime;
            CreateTime = message.CreateTime;
            MessageType = message.MessageType;
            Content = (T) message.Content;
            Success = message.Success;
            SendCount = message.SendCount;
        }

        public int Id { get; set; }
        public string Tag { get; set; }
        public SenderStatus Status { get; internal set; }

        public DateTimeOffset CompleteSendTime { get; set; }
        public DateTimeOffset StartSendTime { get; set; }

        /// <summary>
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        public string MessageType { get; set; }

        public T Content { get; set; }

        public bool Success { get; set; }
        public int SendCount { get; }
    }
}