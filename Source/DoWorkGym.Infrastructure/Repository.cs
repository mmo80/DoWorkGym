using System;
using System.Collections.Generic;
using System.Linq;
using DoWorkGym.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace DoWorkGym.Infrastructure
{
    //
    // http://seesharpdeveloper.blogspot.se/2013/09/c-mongodb-repository-implementation.html
    //
    public abstract class Repository<TEntity> where TEntity : EntityBase
    {
        private readonly MongoDatabase db;
        private readonly string _classNameOfT;

        protected Repository()
        {
            var mongo = new BaseMongo();
            db = mongo.GetDatabase();

            Type typeParameterType = typeof(TEntity);
            _classNameOfT = typeParameterType.Name + "s";
        }


        public bool Insert(TEntity entity)
        {
            entity.Id = ObjectId.GenerateNewId();
            return GetCollection().Insert(entity).Ok;
        }


        public bool Update(TEntity entity)
        {
            if (entity.Id == ObjectId.Empty)
            {
                return Insert(entity);
            }

            return GetCollection()
                .Save(entity)
                .DocumentsAffected > 0;
        }


        public bool Delete(ObjectId id)
        {
            return GetCollection()
                .Remove(Query<TEntity>.EQ(e => e.Id, id))
                .DocumentsAffected > 0;
        }

        public bool Delete(TEntity entity)
        {
            return Delete(entity.Id);
        }


        public TEntity GetById(ObjectId id)
        {
            //return GetCollection().FindOne(Query.EQ("_id", ObjectId.Parse(id)));
            return GetCollection().FindOneByIdAs<TEntity>(id);
        }


        public IList<TEntity> FindAll()
        {
            return GetCollection().FindAllAs<TEntity>().ToList();
        }


        protected MongoCollection<TEntity> GetCollection()
        {
            return db.GetCollection<TEntity>(_classNameOfT);
        }
    }
}
