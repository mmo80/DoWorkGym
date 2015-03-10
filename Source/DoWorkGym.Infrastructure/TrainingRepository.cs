using System.Collections.Generic;
using System.Linq;
using DoWorkGym.Model;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace DoWorkGym.Infrastructure
{
    public class TrainingRepository : Repository<Training>
    {
        public List<Training> GetAll()
        {
            return new List<Training>(base.FindAll());
        }


        public List<Training> GetByUser(ObjectId userId)
        {
            var list = base.GetCollection().AsQueryable<Training>()
                .Where(x => x.UserId == userId)
                .ToList();

            return list;
        }


        public void AddTraining(Training training)
        {
            base.Insert(training);
        }


        public void UpdateTraining(Training training)
        {
            base.Update(training);
        }


        public bool DeleteTraining(string id)
        {
            return base.Delete(ObjectId.Parse(id));
        }


        public Training GetById(string id)
        {
            return base.GetById(ObjectId.Parse(id));
        }


        public Training GetByExerciseId(ObjectId exerciseId)
        {
            var item = base.GetCollection().AsQueryable<Training>()
                .FirstOrDefault(e => e.Exercises.Any(i => i.Id == exerciseId));

            return item;
        }


        public bool UpdateExerciseNameById(string exerciseId, string name)
        {
            var query = Query<Training>.Where(e => e.Exercises.Any(i => i.Id == ObjectId.Parse(exerciseId)));
            var update = Update<Training>.Set(e => e.Name, name); // update modifiers

            return base.GetCollection().Update(query, update)
                .DocumentsAffected > 0;
        }
    }
}
