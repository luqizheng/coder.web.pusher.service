using System;

namespace Coder.WebPusherService
{
    public class NotifyMessageException : Exception
    {
        public NotifyMessageException(string message) : base(message)
        {
        }
    }
}