using System;
using Priority_Queue;

namespace Coder.WebPusherService.Queues
{
    public class NotifyMessageNode : FastPriorityQueueNode
    {
        private readonly DateTimeOffset _nextTimeToRun;

        public NotifyMessageNode(NotifyMessage message, DateTimeOffset nextTimeToRun)
        {
            _nextTimeToRun = nextTimeToRun;
            Message = message;
        }

        public NotifyMessage Message { get; }

        public bool CanRun => DateTimeOffset.Now > _nextTimeToRun;
    }
}