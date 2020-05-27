using System;
using System.Threading;
using Priority_Queue;

namespace Coder.WebPusherService.Queues
{
    internal class NotifyQueueService
    {
        private readonly IServiceProvider _provider;
        private readonly NotifyQueue _queue;
        private readonly Timer _timer;

        public NotifyQueueService(NotifyQueue queue, IServiceProvider provider)
        {
            _timer = new Timer(Execute, null, 10000, 2000);
            _queue = queue;
            _provider = provider;
        }

        private void Execute(object state)
        {
            var manager = _provider.GetService(typeof(NotifyMessageManager)) as NotifyMessageManager;
            while (_queue.Count != 0)
            {
                var first = _queue.First;
                if (first.CanRun)
                {
                    var item = _queue.Dequeue();
                    manager.Send(item.Message);
                }
                else
                {
                    break;
                }
            }
        }
    }

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