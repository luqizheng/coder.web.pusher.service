 using Coder.WebPusherService.Senders.HttpSender;
using Microsoft.AspNetCore.Mvc;

namespace Coder.WebPusherService.Controllers
{
    [ApiController]
    [Route("pusher-service/[controller]")]
    public class HttpNotifyMessageController : Controller
    {
        private readonly NotifyMessageManager _manager;

        public HttpNotifyMessageController(NotifyMessageManager manager)
        {
            _manager = manager;
        }

        [HttpPost("send")]
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
            if (notifyMessage == null)
                return NotFound();
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