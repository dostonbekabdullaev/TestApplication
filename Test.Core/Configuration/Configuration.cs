namespace Test.Core.Configuration
{
    public class Configuration
    {
        public bool IsRedisEnabled { get; set; }
        public string RedisPrimaryEndpoint { get; set; }
        public string RedisReaderEndpoint { get; set;}
    }
}
