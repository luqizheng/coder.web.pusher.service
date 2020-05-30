using System;

namespace Coder.WebPusherService
{
    public enum SenderStatus
    {
        Wait,
        Sending,
        Sent
    }


    public class NotifySettingException : Exception
    {
        public NotifySettingException(string message)
        : base(message)
        {

        }
    }
}