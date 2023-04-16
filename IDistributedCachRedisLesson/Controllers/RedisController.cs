using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace IDistributedCachRedisLesson.Controllers
{
    public class RedisController : Controller
    {
        private IDistributedCache _distributedCache;
        private const string RedisMessage = "hello";
        private const string RedisKey = "newName";
        public RedisController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.AbsoluteExpiration = DateTime.Now.AddSeconds(30);
            cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(10);

            await _distributedCache.SetStringAsync("Myname", "mehmet",cacheOptions);
           
            //var data = Encoding.UTF8.GetBytes(RedisMessage);
            //await _distributedCache.SetAsync(RedisKey, data);

            //var cachedData = await _distributedCache.GetAsync(RedisKey);
            //var cachedMessage = Encoding.UTF8.GetString(cachedData);
            //ViewBag.cached = cachedMessage;

            return View();
        }

        public async Task<IActionResult> ShowAsync()
        {
            //cachedeki veriyi gösterme
            var name = await _distributedCache.GetStringAsync("Myname");
            ViewBag.cached = name;
            return View();
        }

        public IActionResult Remove()
        {
            //cachedeki veriyi silme
            _distributedCache.Remove("Myname");
            return View();
        }

        public IActionResult ImageCache()
        {
            //resmi cache'e kaydetme.
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profil.png");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("imageFile",imageByte);
            return View();
        }
        public IActionResult GetImageCache()
        {
            //resmi cache'e kaydetme.
            byte[] pictureByte =  _distributedCache.Get("imageFile");
            return File(pictureByte ,"image/jpg"); //burdaki image/jpg file'in ne türden olduğunu belirtmemiz demek.
            
        }
    }
}
