using Coder.WebPusherService;
using Coder.WebPusherService.Senders.HttpSender;
using Microsoft.AspNetCore.Mvc;

namespace Coder.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpNotifyMessageController : Controller
    {
        private readonly NotifyMessageManager _manager;

        public HttpNotifyMessageController(NotifyMessageManager manager)
        {
            _manager = manager;
        }

        [HttpPost("send/{messageType}")]
        public IActionResult Send([FromBody] HttpNotifyMessageViewModel content, [FromRoute] string messageType)
        {
            var message = new NotifyMessage(messageType)
            {
                Tag = content.Tag,
                Content = content.Content
            };
            _manager.Send(message).Wait();
            return Ok(message);
        }

        [HttpGet("get/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var notifyMessage = _manager.GetById(id);

            return Ok(
                notifyMessage
            );
        }

        [HttpGet("retry/{id}")]
        public IActionResult Retry([FromRoute] int id)
        {
            var emssge = _manager.Retry(id);

            return Ok(emssge);
        }

        [HttpGet("list")]
        public IActionResult ListByTag([FromQuery] string tag)
        {
            return Ok(_manager.FindByTag(tag));
        }
    }

    public class HttpNotifyMessageViewModel
    {
        public string Tag { get; set; }
        public HttpDictionaryContent Content { get; set; }
    }
}