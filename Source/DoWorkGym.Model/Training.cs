using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace DoWorkGym.Model
{
    public class Training : EntityBase
    {
        public string Name { get; set; }
        public IList<Exercise> Exercises { get; set; }
        public ObjectId UserId { get; set; }

        public Exercise GetExercise(string id)
        {
            if (Exercises.Any())
            {
                var list = new List<Exercise>(Exercises);
                var exercise = list.Find(x => x.Id == ObjectId.Parse(id));

                return exercise;
            }
            return null;
        }
    }
}
