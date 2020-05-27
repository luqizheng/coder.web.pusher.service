using System;

namespace Coder.WebPusherService
{
    public class NotifyMessage
    {
        public NotifyMessage()
        {
            CreateTime = DateTimeOffset.Now;

        }

        public NotifyMessage(string messageType) : this()
        {
            this.MessageType = messageType;
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

        public INotifyContent Content { get; set; }

        public bool Success { get; set; }
        public int SendCount { get; private set; }

        public void Sent(bool sentSuccess)
        {
            Success = sentSuccess;
            Status = sentSuccess ? SenderStatus.Sent : SenderStatus.Wait;
            CompleteSendTime = DateTimeOffset.Now;
        }

        public void StartSend()
        {
            StartSendTime = DateTimeOffset.Now;
            Status = SenderStatus.Sending;
            SendCount++;
        }
    }
}