using System;
using System.Collections.Generic;
using System.Linq;
using DoWorkGym.Model;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace DoWorkGym.Infrastructure
{
    public class WorkoutRepository : Repository<Workout>
    {
        public void AddWorkout(Workout workout)
        {
            base.Insert(workout);
        }


        public IList<Workout> ByExerciseId(string exerciseId)
        {
            var list = base.GetCollection().AsQueryable<Workout>()
                .Where(e => e.Exercise.Id == ObjectId.Parse(exerciseId))
                .ToList();

            return list;
        }


        public void UpdateExerciseNameById(string exerciseId, string name)
        {
            var query = Query<Workout>.EQ(x => x.Exercise.Id, ObjectId.Parse(exerciseId));
            var update = Update<Workout>.Set(e => e.Exercise.Name, name); // update modifiers

            base.GetCollection().Update(query, update);
        }


        public bool DeleteWorkout(string id)
        {
            return base.Delete(ObjectId.Parse(id));
        }


        public DateTime GetLastWorkoutDateByExerciseId(string exerciseId)
        {
            var result =
                base.GetCollection().AsQueryable<Workout>()
                .Where(e => e.Exercise.Id == ObjectId.Parse(exerciseId))
                .OrderByDescending(e => e.Date);

            if (!result.Any())
            {
                return DateTime.MinValue;
            }
            return result.First().Date;
        }


        public IList<Workout> ByUser(ObjectId userId)
        {
            var list = base.GetCollection().AsQueryable<Workout>()
                .Where(x => x.UserId == userId)
                .ToList();

            return list;
        }
    }
}
