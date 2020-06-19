using System;
using System.Threading.Tasks;

namespace Coder.WebPusherService
{
    public enum NotifySettingStatus
    {
        /// <summary>
        /// 正常收发
        /// </summary>
        Normal,
        /// <summary>
        /// 停止接受，队列中的发完就算了
        /// </summary>
        Stop,
        /// <summary>
        /// 暂停，接受转发数据，但是不转发。
        /// </summary>
        Pause,
    }
    public abstract class NotifySettingBase
    {
        protected NotifySettingBase()
        {
        }

        protected NotifySettingBase(string messageType)
        {
            if (messageType == null) throw new ArgumentNullException(nameof(messageType));
            MessageType = messageType;
        }
        public NotifySettingStatus Status { get; protected set; }
        public int Id { get; set; }
        public string MessageType { get; set; }
        public int RetrySpreadSeconds { get; set; } = 20;
        public int MaxRetry { get; set; } = 3;

        public abstract Task<bool> Send(NotifyMessage message);

        public void Stop()
        {
            this.Status = NotifySettingStatus.Stop;
        }


        public void Pause()
        {
            this.Status = NotifySettingStatus.Pause;
        }

        public void Resume()
        {
            this.Status = NotifySettingStatus.Normal;
        }
    }
}