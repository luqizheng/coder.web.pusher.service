using Coder.WebPusherService.Senders.HttpSender;
using Coder.WebPusherService.Senders.HttpSender.ViewModel;
using Coder.WebPusherService.Stores;
using Coder.WebPusherService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Coder.WebPusherService.Controllers.Manage
{
    [ApiController]
    [Route("pusher-service/manage/[controller]")]
    public class HttpNotifySettingController : Controller
    {
        private readonly INotifySettingStore _settingStore;

        public HttpNotifySettingController(INotifySettingStore settingStore)
        {
            _settingStore = settingStore;
        }

        [HttpGet("{id:int}")]
        public IActionResult Get([FromRoute] int id)
        {
            var result = _settingStore.GetById<HttpNotifySetting>(id);
            if (result == null)
                return NotFound();
            return Json(new HttpNotifySettingDetailViewModel(result));
        }

        [HttpGet("list")]
        public IActionResult List(string messageType, int page, int pageSize)
        {
            var result = _settingStore.List<HttpNotifySetting>(messageType, page, pageSize, out var total);
            return Json(new { data = result, total });
        }

        [HttpPost("save")]
        public IActionResult Save([FromBody] HttpNotifySettingDetailViewModel notifySettingBase)
        {
            var result = notifySettingBase.Id != 0
                ? _settingStore.GetById<HttpNotifySetting>(notifySettingBase.Id)
                : new HttpNotifySetting();
            result.Url = notifySettingBase.Url;
            result.SendType = notifySettingBase.SendType;
            result.Method = notifySettingBase.Method;
            result.SendContentType = notifySettingBase.ContentType;
            result.SubmitDataTemplate = notifySettingBase.SubmitDataTemplate;
            result.RawContentTemplate = notifySettingBase.RawContentTemplate;

            result.RetrySpreadSeconds = notifySettingBase.RetrySpreadSeconds;
            result.MaxRetry = notifySettingBase.MaxRetry;
            result.MessageType = notifySettingBase.MessageType;
            try
            {
                _settingStore.SaveOrUpdate(result);
                return Json(new NotifyResult { Id = result.Id, Success = true });
            }
            catch (NotifySettingException ex)
            {
                return Json(new NotifyResult { Id = notifySettingBase.Id, Success = false, Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _settingStore.DeleteById(id);
            return Json(new { success = true });
        }
    }
}
