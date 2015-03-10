using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DoWorkGym.Infrastructure;
using DoWorkGym.Model.Cache;

namespace DoWorkGym.Service.Helpers
{
    // url: http://www.nullskull.com/a/1471/a-mongodb-cache-utility.aspx
    public class MongoCache
    {
        private const int DefaultExpireDateHours = 24;

        private CacheRepository _cacheRepository;
        private CacheRepository CacheRepo
        {
            get { return _cacheRepository ?? (_cacheRepository = new CacheRepository()); }
        }


        public void Clear(string key)
        {
            Remove(key);
        }


        public T Get<T>(string key)
        {
            try
            {
                return (T)this[key];
            }
            catch
            {
                return default(T);
            }
        }


        public bool Get<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value = default(T);
                    return false;
                }

                value = (T)this[key];
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }


        public bool Exists(string key)
        {
            return CacheRepo.GetById(key) != null;
        }


        public object this[string cacheKey]
        {
            get { return Get(cacheKey); }
            set { Add(cacheKey, value, DateTime.Now.AddHours(DefaultExpireDateHours)); }
        }


        public object Add(string key, object entry, DateTime utcExpiry)
        {
            Set(key, entry, utcExpiry);
            return entry;
        }



        private object Get(string key)
        {
            CacheItem item = CacheRepo.GetById(key);
            if (item == null || item.Expires.ToLocalTime() <= DateTime.Now)
            {
                Remove(key);
                return null;
            }
            var f = new BinaryFormatter();
            var ms = new MemoryStream(item.Item);
            object o = f.Deserialize(ms);
            return o;
        }


        private void Remove(string key)
        {
            var q = new CacheItem
            {
                CacheKey = key
            };
            CacheRepo.Delete(q);
        }


        private void Set(string key, object entry, DateTime utcExpiry)
        {
                var f = new BinaryFormatter();
                var ms = new MemoryStream();
                f.Serialize(ms, entry);
                var q = new CacheItem
                {
                    CacheKey = key,
                    Expires = utcExpiry,
                    Item = ms.ToArray()
                };
                CacheRepo.Save(q);
        }
    }
}
