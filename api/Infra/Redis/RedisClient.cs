namespace api.Infra.Redis
{
    using StackExchange.Redis;
    public class RedisClient : IRedisClient
    {
        private static ConnectionMultiplexer _CONN;
        
        static RedisClient()
        {
            _CONN = ConnectionMultiplexer.ConnectAsync("localhost").ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public IDatabase GetDatabase(int db = -1, object asyncState = null)
        {
            return _CONN.GetDatabase(db, asyncState);
        }
    }
}