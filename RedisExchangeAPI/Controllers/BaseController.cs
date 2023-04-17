using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Controllers
{
    public class BaseController : Controller
    {
        protected readonly RedisService _redisService;
        protected readonly IDatabase db;

        public BaseController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDatabase(0);
        }
    }
}
