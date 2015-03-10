using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DoWorkGym.Model.Cache
{
    [Serializable]
    public class CacheItem
    {
        [BsonId]
        public string CacheKey { get; set; }
        public DateTime Expires { get; set; }
        public byte[] Item { get; set; }
    }
}
