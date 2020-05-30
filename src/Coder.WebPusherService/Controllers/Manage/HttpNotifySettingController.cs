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
            return Ok(new HttpNotifySettingDetailViewModel(result));
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
            result.ContentType = notifySettingBase.ContentType;
            result.SubmitDataTemplate = notifySettingBase.SubmitDataTemplate;
            result.RawContentTemplate = notifySettingBase.RawContentTemplate;

            result.RetrySpreadSeconds = notifySettingBase.RetrySpreadSeconds;
            result.MaxRetry = notifySettingBase.MaxRetry;
            result.MessageType = notifySettingBase.MessageType;
            try
            {
                _settingStore.SaveOrUpdate(result);
                return Ok(new NotifyResult { Id = notifySettingBase.Id, Success = true });
            }
            catch (NotifySettingException ex)
            {
                return Ok(new NotifyResult { Id = notifySettingBase.Id, Success = false, Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _settingStore.DeleteById(id);
            return Ok(new { success = true });
        }
    }
}