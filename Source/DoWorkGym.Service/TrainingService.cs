using System.Collections.Generic;
using DoWorkGym.Infrastructure;
using DoWorkGym.Model;

namespace DoWorkGym.Service
{
    public class TrainingService
    {
        private TrainingRepository _trainingRepository;
        private TrainingRepository TrainingRepository
        {
            get { return _trainingRepository ?? (_trainingRepository = new TrainingRepository()); }
        }


        public Dictionary<string, string> GetTrainingAndExerciseNames(string trainingId, string exerciseId)
        {
            var list = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(trainingId))
            {
                return null;
            }

            if (string.IsNullOrEmpty(exerciseId))
            {
                return null;
            }

            Training training = TrainingRepository.GetById(trainingId);

            list.Add(trainingId, training.Name);
            list.Add(exerciseId, training.GetExercise(exerciseId).Name);

            return list;
        }


        //public void UpdateExerciseName(string exerciseId, string name)
        //{
        //    TrainingRepository.UpdateExerciseNameById(exerciseId, name);
        //    workoutRepository.UpdateExerciseNameById(exerciseId, name);
        //}
    }
}
