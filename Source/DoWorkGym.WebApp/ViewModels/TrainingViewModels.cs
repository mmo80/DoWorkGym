namespace DoWorkGym.WebApp.ViewModels
{
    namespace TrainingViewModels
    {
        public class TrainingView
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class AddTrainingView
        {
            public string Name { get; set; }
        }

        public class ExerciseView
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class ExercisesViewRequest
        {
            public string Id { get; set; }
        }

        public class AddExercise
        {
            public string TrainingId { get; set; }
            public string Name { get; set; }
        }

        public class RemoveExercise
        {
            public string TrainingId { get; set; }
            public string Id { get; set; }
        }
    }
}