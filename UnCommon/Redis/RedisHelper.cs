using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnCommon.Files;

namespace UnCommon.Redis
{
    /// <summary>
    /// Redis帮助类
    /// </summary>
    public class RedisHelper
    {
        /// <summary>
        /// 客户端链接池管理对象
        /// </summary>
        private readonly PooledRedisClientManager pool = null;

        /// <summary>
        /// 主机链接串:127.0.0.1:6379;
        /// </summary>
        private readonly string[] redisHosts = null;

        /// <summary>
        /// IP
        /// </summary>
        private string IP = null;

        /// <summary>
        /// 端口
        /// </summary>
        private int Port = 0;

        /// <summary>
        /// 读池最大值
        /// </summary>
        private int RedisMaxReadPool = 3;

        /// <summary>
        /// 写池最大值
        /// </summary>
        private int RedisMaxWritePool = 1;

        /// <summary>
        /// 实例化
        /// </summary>
        public RedisHelper(string ip, int port, int maxReadPool, int maxWritePool)
        {
            IP = ip;
            Port = port;
            RedisMaxReadPool = maxReadPool;
            RedisMaxWritePool = maxWritePool;

            var redisHostStr = ip + ":" + port;
            if (!string.IsNullOrEmpty(redisHostStr))
            {
                redisHosts = redisHostStr.Split(',');

                if (redisHosts.Length > 0)
                {
                    pool = new PooledRedisClientManager(redisHosts, redisHosts,
                        new RedisClientManagerConfig()
                        {
                            MaxWritePoolSize = RedisMaxWritePool,
                            MaxReadPoolSize = RedisMaxReadPool,
                            AutoStart = true
                        });
                }
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        private void set<T>(string key, T value, DateTime expiry)
        {
            if (value == null)
            {
                return;
            }

            if (expiry <= DateTime.Now)
            {
                remove(key);
                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetCacheClient())
                    {
                        if (r != null)
                        {
                            //r.SendTimeout = 1000;
                            r.Set(key, value, expiry - DateTime.Now);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnFile.writeLog("set", ex.ToString() + "\r\nredisHosts:" + IP + ":" + Port + "\r\nRedisMaxReadPool:" + RedisMaxReadPool + "\r\nRedisMaxWritePool:" + RedisMaxWritePool + "\r\nkey:" + key + "\r\nvalue:" + value + "\r\nexpiry:" + expiry.ToShortDateString());
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration"></param>
        private void set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (value == null)
            {
                return;
            }

            if (slidingExpiration.TotalSeconds <= 0)
            {
                remove(key);
                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetCacheClient())
                    {
                        if (r != null)
                        {
                            //r.SendTimeout = 1000;
                            r.Set(key, value, slidingExpiration);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnFile.writeLog("set", ex.ToString() + "\r\nredisHosts:" + IP + ":" + Port + "\r\nRedisMaxReadPool:" + RedisMaxReadPool + "\r\nRedisMaxWritePool:" + RedisMaxWritePool + "\r\nkey:" + key + "\r\nvalue:" + value + "\r\nslidingExpiration:" + slidingExpiration.Seconds);
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <param name="expire">过期时间(秒)</param>
        public void set<T>(string key, T value, int? expire)
        {
            set<T>(key, value, DateTime.Now.AddSeconds(expire.Value));
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }
            T obj = default(T);
            try
            {
                if (pool != null)
                {
                    // 只读链接
                    using (var r = pool.GetCacheClient())
                    {
                        if (r != null)
                        {
                            //r.SendTimeout = 1000;
                            obj = r.Get<T>(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnFile.writeLog("get", ex.ToString() + "\r\nredisHosts:" + IP + ":" + Port + "\r\nRedisMaxReadPool:" + RedisMaxReadPool + "\r\nRedisMaxWritePool:" + RedisMaxWritePool + "\r\n" + key);
            }
            return obj;
        }

        /// <summary>
        /// 锁定键
        /// </summary>
        public IDisposable acquireLock(string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            return r.AcquireLock(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnFile.writeLog("acquireLock", ex.ToString() + "\r\nredisHosts:" + IP + ":" + Port + "\r\n:" + RedisMaxReadPool + "\r\nRedisMaxWritePool:" + RedisMaxWritePool + "\r\nkey:" + key);
            }
            return null;
        }

        /// <summary>
        /// 删除DB数据
        /// </summary>
        public void flushDb()
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.FlushDb();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnFile.writeLog("flushDb", ex.ToString() + "\r\nredisHosts:" + IP + ":" + Port + "\r\n:" + RedisMaxReadPool + "\r\nRedisMaxWritePool:" + RedisMaxWritePool);
            }
        }

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public void flushAll()
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.FlushAll();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnFile.writeLog("flushAll", ex.ToString() + "\r\nredisHosts:" + IP + ":" + Port + "\r\n:" + RedisMaxReadPool + "\r\nRedisMaxWritePool:" + RedisMaxWritePool);
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public void remove(string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Remove(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnFile.writeLog("remove", ex.ToString() + "\r\nredisHosts:" + IP + ":" + Port + "\r\n:" + RedisMaxReadPool + "\r\nRedisMaxWritePool:" + RedisMaxWritePool + "\r\nkey:" + key);
            }
        }

        /// <summary>
        /// KEY是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool exists(string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            return r.ContainsKey(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnFile.writeLog("exists", ex.ToString() + "\r\nredisHosts:" + IP + ":" + Port + "\r\nRedisMaxReadPool:" + RedisMaxReadPool + "\r\nRedisMaxWritePool:" + RedisMaxWritePool + "\r\nkey:" + key);
            }
            return false;
        }

        /// <summary>
        /// 获取所有键
        /// </summary>
        /// <returns></returns>
        public List<string> getAllKeys()
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            return r.GetAllKeys();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnFile.writeLog("getAllKeys", ex.ToString() + "\r\nredisHosts:" + redisHosts + "\r\n:" + RedisMaxReadPool + "\r\nRedisMaxWritePool:" + RedisMaxWritePool);
            }
            return new List<string>();
        }

        /// <summary>
        /// 匹配所有键
        /// </summary>
        /// <param name="pattern">正则</param>
        /// <returns></returns>
        public List<string> getKeysByPattern(string pattern)
        {
            List<string> list = new List<string>();
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            Regex regex = new Regex(pattern);
                            var keys = r.GetAllKeys();
                            foreach (var item in keys)
                            {
                                if (regex.IsMatch(item))
                                {
                                    list.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnFile.writeLog("getRedisKeysByPattern", ex.ToString() + "\r\nredisHosts:" + IP + ":" + Port + "\r\nRedisMaxReadPool:" + RedisMaxReadPool + "\r\nRedisMaxWritePool:" + RedisMaxWritePool);
            }
            return list;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="pattern">正则</param>
        public void removeByRegex(string pattern)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.RemoveAll(getKeysByPattern(pattern));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnFile.writeLog("removeByRegex", ex.ToString() + "\r\nredisHosts:" + IP + ":" + Port + "\r\nRedisMaxReadPool:" + RedisMaxReadPool + "\r\nRedisMaxWritePool:" + RedisMaxWritePool);
            }
        }
    }
}
