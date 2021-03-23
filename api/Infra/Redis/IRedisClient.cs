namespace api.Infra.Redis
{
    using StackExchange.Redis;

    public interface IRedisClient
    {
        IDatabase GetDatabase(int db = -1, object asyncState = null);
    }
}