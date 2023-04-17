using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Controllers
{
    public class RedisController : BaseController
    {
        private readonly string listkey = "listname";
        public string hashKey { get; set; } = "dictionary";
        public RedisController(RedisService redisService):base(redisService)
        {
        }

        public IActionResult Index()
        {
            db.StringSet("rediServiceName","Mehmet Dokuyucu");
            db.StringSet("guest", 100);
            return View();
        }
        public IActionResult Show()
        {
            var servicenameValue = db.StringGet("rediServiceName");
            if (servicenameValue.HasValue)
                ViewBag.value = servicenameValue.ToString();
            return View();
        }
        public IActionResult ListTypeIndex()
        {
            List<string> nameList = new List<string>();
            if (db.KeyExists(listkey))
            {
                db.ListRange(listkey).ToList().ForEach(x =>
                {
                    nameList.Add(x.ToString());
                });
            }
            return View(nameList);
        }
        public IActionResult ListTypeAdd(string name)
        {
            db.ListLeftPush(listkey, name); //listenin başına veri ekleme listrightpush en sona ekler.
            return RedirectToAction("ListTypeIndex");
        }
        public async Task<IActionResult> DeleteListType(string name)
        {
            await db.ListRemoveAsync(listkey,name);
            return RedirectToAction("ListTypeIndex");
        }
        public IActionResult HashTypeIndex()
        {
            Dictionary<string,string> list = new Dictionary<string,string>();

            if (db.KeyExists(hashKey))
            {
                db.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }

            return View(list);
        }
        public IActionResult HashTypeAdd(string key, string value)
        {
            db.HashSet(hashKey,key,value);
            return RedirectToAction("HashTypeIndex");
        }

        public async Task<IActionResult> DeleteHashType(string name)
        {
            await db.HashDeleteAsync(hashKey, name);
            return RedirectToAction("HashTypeIndex");
        }
    }
}
