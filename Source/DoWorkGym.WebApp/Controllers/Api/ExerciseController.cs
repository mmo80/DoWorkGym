using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DoWorkGym.Infrastructure;
using DoWorkGym.Model;
using DoWorkGym.Service;
using DoWorkGym.WebApp.Security;
using DoWorkGym.WebApp.ViewModels.TrainingViewModels;
using MongoDB.Bson;

namespace DoWorkGym.WebApp.Controllers.Api
{
    [ApiAuthorize]
    public class ExerciseController : ApiController
    {
        private AccountService _accountService;
        private AccountService AccountService
        {
            get { return _accountService ?? (_accountService = new AccountService()); }
        }

        private TrainingRepository _trainingRepository;
        private TrainingRepository TrainingRepository
        {
            get { return _trainingRepository ?? (_trainingRepository = new TrainingRepository()); }
        }


        public IEnumerable<TrainingView> GetTrainingList()
        {
            User user = AccountService.GetAuthorizedUser();

            var list = TrainingRepository.GetByUser(user.Id);

            return list.Select(training => new TrainingView()
            {
                Id = training.Id.ToString(),
                Name = training.Name
            }).ToList();
        }


        public IEnumerable<ExerciseView> GetExerciseByTraining([FromUri]ExercisesViewRequest exercisesViewRequest)
        {
            var training = TrainingRepository.GetById(exercisesViewRequest.Id);

            var exercises = training.Exercises;

            if (exercises != null)
            {
                return exercises.Select(exercise => new ExerciseView()
                {
                    Id = exercise.Id.ToString(),
                    Name = exercise.Name
                }).ToList();
            }
            return null;
        }


        public HttpResponseMessage AddExercise(AddExercise addExercise)
        {
            var training = TrainingRepository.GetById(addExercise.TrainingId);

            if (training.Exercises == null)
            {
                training.Exercises = new List<Exercise>();
            }

            training.Exercises.Add(new Exercise()
            {
                Id = ObjectId.GenerateNewId(),
                Name = addExercise.Name
            });

            TrainingRepository.UpdateTraining(training);

            return Request.CreateResponse(HttpStatusCode.Created);
        }


        public HttpResponseMessage DeleteExercise(RemoveExercise removeExercise)
        {
            var training = TrainingRepository.GetById(removeExercise.TrainingId);

            if (training.Exercises == null || training.Exercises.Count <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }

            var exerciseId = ObjectId.Parse(removeExercise.Id);
            var exerciseList = new List<Exercise>(training.Exercises);
            if (exerciseList.Exists(e => e.Id == exerciseId))
            {
                exerciseList.RemoveAll(e => e.Id == exerciseId);
                training.Exercises = exerciseList;
            }

            TrainingRepository.UpdateTraining(training);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
