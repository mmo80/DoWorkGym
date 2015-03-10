using System;
using MongoDB.Bson;

namespace DoWorkGym.Model
{
    public class Workout : EntityBase
    {
        public Exercise Exercise { get; set; }
        public DateTime Date { get; set; }
        public int Set { get; set; }
        public decimal Weight { get; set; }
        public int Reps { get; set; }
        public string Unit { get; set; }
        public string Note { get; set; }
        public ObjectId UserId { get; set; }
    }
}
