using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Services;

namespace RedisExchangeAPI.Controllers
{
    public class RedisController : Controller
    {
        private readonly RedisService _redisService;

        public RedisController(RedisService redisService)
        {
            _redisService = redisService;
        }

        public IActionResult Index()
        {
            var db = _redisService.GetDatabase(0);
            db.StringSet("rediServiceName","Mehmet Dokuyucu");
            db.StringSet("guest", 100);
            return View();
        }
        public IActionResult Show()
        {
            var db = _redisService.GetDatabase(0);
            var servicenameValue = db.StringGet("rediServiceName");
            if (servicenameValue.HasValue)
                ViewBag.value = servicenameValue.ToString();
            return View();
        }
    }
}
