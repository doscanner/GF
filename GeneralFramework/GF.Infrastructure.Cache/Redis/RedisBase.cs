using StackExchange.Redis;
using System;
using System.Configuration;

namespace GF.Infrastructure.Cache.Redis
{
    public class RedisBase
    {
        private static string[] readWriteHosts = ConfigurationManager.AppSettings["readWriteHosts"].Split('@');

        private static ConnectionMultiplexer _redis;
        private static readonly Object _multiplexerLock = new Object();

        private static void ConnectRedis()
        {
            try
            {
                var options = new ConfigurationOptions();
                options.EndPoints.Add(readWriteHosts[1]);
                options.Ssl = false;
                options.Password = readWriteHosts[0];
                // needed for FLUSHDB command
                options.AllowAdmin = true;
                options.AbortOnConnectFail = false;
                options.ConnectRetry = 5;

                // necessary?
                options.KeepAlive = 180;
                options.ConnectTimeout = 15000;
                options.SyncTimeout = 15000;

                _redis = ConnectionMultiplexer.Connect(options);
            }
            catch (Exception ex)
            {
                throw ex;
                //exception handling goes here
            }
        }

        private static ConnectionMultiplexer RedisMultiplexer
        {
            get
            {
                lock (_multiplexerLock)
                {
                    if (_redis == null || !_redis.IsConnected)
                    {
                        ConnectRedis();
                    }
                    return _redis;
                }
            }
        }

        public static IDatabase cache
        {
            get
            {
                return RedisMultiplexer.GetDatabase();
            }
        }

        public static bool Item_Set<T>(string key, T value)
        {
            var v = StackExchangeRedisExtensions.Serialize(value);
            return cache.StringSet(key, v);
        }

        public static bool Item_Set<T>(string key, T value, TimeSpan expiresIn)
        {
            var v = StackExchangeRedisExtensions.Serialize(value);
            return cache.StringSet(key, v, expiresIn);
        }

        public static T Item_Get<T>(string key) where T : class
        {
            var v = cache.StringGet(key);
            if (string.IsNullOrWhiteSpace(v))
            {
                //("InfoLog", "\r\nkey:" + key + "\r\n value:" + v);
            }
            return StackExchangeRedisExtensions.Deserialize<T>(v);
        }

        public static bool Item_Remove(string key)
        {
            return cache.KeyDelete(key);
        }

        public static bool Hash_Set<T>(string key, string dataKey, T value)
        {
            var v = StackExchangeRedisExtensions.Serialize(value);
            return cache.HashSet(key, dataKey, v);
        }

        public static bool Hash_Remove(string key, string dataKey)
        {
            return cache.HashDelete(key, dataKey);
        }

        public static T Hash_Get<T>(string key, string dataKey)
        {
            var v = cache.HashGet(key, dataKey);
            return StackExchangeRedisExtensions.Deserialize<T>(v);
        }
    }
}
