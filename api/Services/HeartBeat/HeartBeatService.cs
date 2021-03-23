namespace api.Services.HeartBeat
{
    using System;
    using System.Threading.Tasks;
    using Infra.Redis;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Linq;

    public class HeartBeatService : IHeartBeatService
    {
        private readonly IRedisClient _redisClient;
        private readonly IOptionsMonitor<HeartBeatOptions> _options;

        public HeartBeatService(IRedisClient redis, IOptionsMonitor<HeartBeatOptions> options)
        {
            _redisClient = redis;
            _options = options;
        }

        public async Task ReportAliveAsync(string groupId, string id)
        {
            var db = _redisClient.GetDatabase();
            await db.HashSetAsync(groupId, new StackExchange.Redis.HashEntry[] 
            {
                new StackExchange.Redis.HashEntry(id, DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            });
        }

        public async Task<IEnumerable<string>> GetAliveMembersAsync(string groupId)
        {
            var db = _redisClient.GetDatabase();
            var members = await db.HashGetAllAsync(groupId);
            var addtion = _options.CurrentValue.Interval * _options.CurrentValue.ToleranceCount;
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var res = members.Where(x => (long.Parse(x.Value) + addtion) >= now).Select(x => x.Name.ToString());
            return res;
        }
    }
}