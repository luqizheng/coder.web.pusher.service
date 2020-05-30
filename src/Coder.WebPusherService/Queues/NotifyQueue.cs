using System;
using Priority_Queue;

namespace Coder.WebPusherService.Queues
{
    public class NotifyQueue
    {
        private readonly FastPriorityQueue<NotifyMessageNode>
            _queue = new FastPriorityQueue<NotifyMessageNode>(64000);


        public int Count => _queue.Count;

        public NotifyMessageNode First => _queue.First;

        public void Add(NotifyMessage message, DateTimeOffset nextRunTime)
        {
            //less is high priority
            var priority = (int) (DateTimeOffset.Now - nextRunTime).TotalSeconds;
            if (priority < 0)
                priority = 0;
            _queue.Enqueue(new NotifyMessageNode(message, nextRunTime), priority);
        }


        public NotifyMessageNode Dequeue()
        {
            return _queue.Dequeue();
        }
    }
}