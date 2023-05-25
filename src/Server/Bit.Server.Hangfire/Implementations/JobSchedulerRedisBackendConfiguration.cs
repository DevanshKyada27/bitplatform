﻿using Hangfire.Redis.StackExchange;
using StackExchange.Redis;

namespace Bit.Hangfire.Implementations
{
    public class JobSchedulerRedisBackendConfiguration : JobSchedulerBaseBackendConfiguration<RedisStorage>
    {
        public virtual IConnectionMultiplexer ConnectionMultiplexer { get; set; } = default!;

        protected override RedisStorage BuildStorage()
        {
            RedisStorage storage = new RedisStorage(ConnectionMultiplexer);

            return storage;
        }
    }
}
