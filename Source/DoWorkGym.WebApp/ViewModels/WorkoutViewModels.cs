
using System;
using System.Collections.Generic;

namespace DoWorkGym.WebApp.ViewModels
{
    namespace WorkoutViewModels
    {
        public class TrainingExerciseView
        {
            public string Training { get; set; }
            public string Exercise { get; set; }
        }

        public class WorkoutItemView
        {
            public string TrainingId { get; set; }
            public string ExerciseId { get; set; }
            public string Date { get; set; }
            public int Set { get; set; }
            public decimal Weight { get; set; }
            public int Reps { get; set; }
            public string Unit { get; set; }
            public string Note { get; set; }
        }

        public class RequestWorkoutList
        {
            public string ExerciseId { get; set; }
        }

        public class ResponseWorkoutItem
        {
            public string Id { get; set; }
            public string Date { get; set; }
            public List<WorkoutItemDateRow> DateRows { get; set; }
        }

        public class WorkoutItemDateRow
        {
            public string Id { get; set; }
            public string Time { get; set; }
            public int Set { get; set; }
            public decimal Weight { get; set; }
            public int Reps { get; set; }
            public string Unit { get; set; }
            public string Note { get; set; }
        }

        public class WorkoutView
        {
            public string Id { get; set; }
        }

        public class WorkoutExercisesViewRequest
        {
            public string TrainingId { get; set; }
        }

        public class WorkoutExerciseView
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LastWorkoutDate { get; set; }
            public int Count { get; set; }
        }
    }
}