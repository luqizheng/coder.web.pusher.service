using Coder.WebPusherService.Senders.HttpSender;
using Coder.WebPusherService.Senders.HttpSender.ViewModel;
using Coder.WebPusherService.Stores;
using Microsoft.AspNetCore.Mvc;

namespace Coder.WebApi.Controllers.Manage
{
    [ApiController]
    [Route("manage/[controller]")]
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
            return Ok(new {success = true});
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _settingStore.DeleteById(id);
            return Ok(new {success = true});
        }
    }
}