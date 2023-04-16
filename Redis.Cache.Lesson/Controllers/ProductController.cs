using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Redis.Cache.Lesson.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            //1.yol
            //_memoryCache.Set<string>("zaman",DateTime.Now.ToString());
            //2.yol
            if (!_memoryCache.TryGetValue("zaman", out string cachdata))
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                //absolute expiration örnek - direkt verilen süre kadar memoryde kalır.
                //options.AbsoluteExpiration = DateTime.Now.AddSeconds(20); // memoryde olacak datanın süresini veriyoruz.10 saniye boyunca cachede duracak.

                //sliding expiration - Verilen süre içerisinde cache verisine erişilirse, Cache in süresi siliding time süresi kadar daha uzatılmış olur.(Sadece bu özellik kullanılırsa her zaman eski dataya ulaşılabilir.
                //Bu sorunu engellemek için sliding time la birlikte absolute time da belirtmek gerekir.)
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
                options.SlidingExpiration = TimeSpan.FromSeconds(10);
                options.Priority=CacheItemPriority.High; //burada verinin önemine göre öncelik belirtebiliriz. O sıraya göre silip silmeyeceğine karar verir.

                options.RegisterPostEvictionCallback((key,value,reason,state) =>
                {
                    _memoryCache.Set("callback",$"{key} -> {value} -> sebep : {reason}"); //memorydeki verinin neden silindiğini burada elde edebiliyoruz.
                });

                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(),options);
            }
            return View();
        }
        public IActionResult Show()
        {
            _memoryCache.TryGetValue("zaman", out string cachdata); // memoryde veri varsa çekip cachdata değişkenine atacak yoksa da boş dönecek.
            _memoryCache.TryGetValue("callback", out string callbackdata);
            ViewBag.cachedata = cachdata;
            ViewBag.callbackdata = callbackdata;
            return View();
        }
    }
}
