using Microsoft.Extensions.Logging;
using System;
using System.Timers;

namespace Coder.WebPusherService.Queues
{
    public class NotifyQueueService
    {
        private readonly ILogger<NotifyQueueService> _logger;
        private readonly IServiceProvider _provider;
        private readonly NotifyQueue _queue;
        private readonly Timer _timer;

        public NotifyQueueService(IServiceProvider provider, ILogger<NotifyQueueService> logger)
        {
            _timer = new Timer(2000)
            {
                Enabled = true
            };
            _timer.Elapsed += _timer_Elapsed;
            _queue = new NotifyQueue();
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger;
        }

        private bool _fromPersistent = false;
        private void ResumeAll()
        {
            var manager = _provider.GetService(typeof(NotifyMessageManager)) as NotifyMessageManager;
            manager.ResumeAll();
            _fromPersistent = true;
        }
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var manager = _provider.GetService(typeof(NotifyMessageManager)) as NotifyMessageManager;
            _timer.Enabled = false;
            try
            {
                _logger.LogInformation("执行队列中的推送服务,数量" + _queue.Count);
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
            finally
            {
                _logger.LogInformation("执行结束。");
                _timer.Enabled = true;
            }
        }


        public void Push(NotifyMessage message, DateTimeOffset nextRunTime)
        {
            _queue.Add(message, nextRunTime);
        }
    }
}
