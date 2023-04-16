using StackExchange.Redis;

namespace RedisExchangeAPI.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        private ConnectionMultiplexer _multiplexer; // redis dbyle iletişimi sağlayan connection sınıfı
        public IDatabase database { get; set; }
       
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
            Connect();
        }
        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";
            _multiplexer= ConnectionMultiplexer.Connect(configString); // yukarıda appsettings dosyasından aldığımız host ve port bilgileri ile redis serverına bağlandık.

        }
        public IDatabase GetDatabase(int db)
        {
            return _multiplexer.GetDatabase(db);
        }
    }
}
