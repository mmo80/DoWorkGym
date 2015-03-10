using System;
using DoWorkGym.Model.Cache;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace DoWorkGym.Infrastructure
{
    public class CacheRepository
    {
        private readonly MongoDatabase db;
        private readonly string _classNameOfT;

        public CacheRepository()
        {
            var mongo = new BaseMongo();
            db = mongo.GetDatabase();

            Type typeParameterType = typeof(CacheItem);
            _classNameOfT = typeParameterType.Name + "s";
        }


        protected MongoCollection<CacheItem> GetCollection()
        {
            return db.GetCollection<CacheItem>(_classNameOfT);
        }


        public CacheItem GetById(string cacheKey)
        {
            //return GetCollection().FindOne(Query.EQ("_id", cacheKey));
            return GetCollection().FindOneByIdAs<CacheItem>(cacheKey);
        }


        public bool Insert(CacheItem entity)
        {
            //entity.CacheKey = ObjectId.GenerateNewId();
            return GetCollection().Insert(entity).Ok;
        }


        public bool Save(CacheItem entity)
        {
            return GetCollection()
                .Save(entity)
                .DocumentsAffected > 0;
        }


        public bool Delete(CacheItem entity)
        {
            return Delete(entity.CacheKey);
        }


        public bool Delete(string cacheKey)
        {
            return GetCollection()
                .Remove(Query<CacheItem>.EQ(e => e.CacheKey, cacheKey))
                .DocumentsAffected > 0;
        }
    }
}
