using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedisExample.Cache
{
    public class RedisRepository
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisRepository(string url)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
            
        }

        public IDatabase GetDatabase(int dbIndex)
        {
            return _connectionMultiplexer.GetDatabase(dbIndex);
        }
    }
}
