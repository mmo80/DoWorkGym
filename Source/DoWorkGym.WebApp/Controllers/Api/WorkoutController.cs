using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DoWorkGym.Infrastructure;
using DoWorkGym.Model;
using DoWorkGym.Service;
using DoWorkGym.WebApp.Security;
using DoWorkGym.WebApp.ViewModels.WorkoutViewModels;

namespace DoWorkGym.WebApp.Controllers.Api
{
    [ApiAuthorize]
    public class WorkoutController : ApiController
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

        private WorkoutRepository _workoutRepository;
        private WorkoutRepository WorkoutRepository
        {
            get { return _workoutRepository ?? (_workoutRepository = new WorkoutRepository()); }
        }


        public HttpResponseMessage AddWorkout(WorkoutItemView item)
        {
            User user = AccountService.GetAuthorizedUser();
            Training training = TrainingRepository.GetById(item.TrainingId);

            var exercise = training.GetExercise(item.ExerciseId);

            DateTime date = DateTime.Now;
            TimeSpan time = date.TimeOfDay;

            var workout = new Workout()
            {
                Exercise = exercise,
                Date = Convert.ToDateTime(item.Date).ToLocalTime().Add(time),
                Set = item.Set,
                Weight = item.Weight,
                Reps = item.Reps,
                Unit = item.Unit,
                Note = item.Note,
                UserId = user.Id
            };

            var workoutRepository = new WorkoutRepository();
            workoutRepository.AddWorkout(workout);

            return Request.CreateResponse(HttpStatusCode.Created);
        }


        public IList<ResponseWorkoutItem> GetWorkouts([FromUri]RequestWorkoutList request)
        {
            var list = WorkoutRepository.ByExerciseId(request.ExerciseId);

            list = list.OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Set)
                .ToList();

            var returnList = new List<ResponseWorkoutItem>();
            var prevDate = new DateTime();
            var item = new ResponseWorkoutItem();
            foreach (var workout in list)
            {
                if (!(prevDate.Year.Equals(workout.Date.Year) &&
                    prevDate.Month.Equals(workout.Date.Month) &&
                    prevDate.Day.Equals(workout.Date.Day)))
                {
                    item = new ResponseWorkoutItem()
                    {
                        Id = workout.Id.ToString(),
                        Date = workout.Date.ToString("yyyy-MM-dd"),
                        DateRows = new List<WorkoutItemDateRow>()
                    };

                    returnList.Add(item);
                }

                item.DateRows.Add(new WorkoutItemDateRow()
                {
                    Id = workout.Id.ToString(),
                    Time = workout.Date.ToString("HH:mm"),
                    Reps = workout.Reps,
                    Set = workout.Set,
                    Weight = workout.Weight,
                    Unit = workout.Unit,
                    Note = workout.Note
                });

                prevDate = workout.Date;
            }

            return returnList;
        }


        public HttpResponseMessage DeleteWorkout(WorkoutView workoutView)
        {
            WorkoutRepository.DeleteWorkout(workoutView.Id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }


        public IEnumerable<WorkoutExerciseView> GetExerciseByTraining([FromUri]WorkoutExercisesViewRequest workoutExercisesViewRequest)
        {
            var training = TrainingRepository.GetById(workoutExercisesViewRequest.TrainingId);

            var exercises = training.Exercises;
            if (exercises != null)
            {
                var list = new List<WorkoutExerciseView>();
                foreach (var exercise in exercises)
                {
                    var lastDate = WorkoutRepository.GetLastWorkoutDateByExerciseId(exercise.Id.ToString());

                    list.Add(new WorkoutExerciseView()
                    {
                        Id = exercise.Id.ToString(),
                        Name = exercise.Name,
                        LastWorkoutDate = lastDate > DateTime.MinValue ? lastDate.ToString("yyyy-MM-dd") : "-",
                        Count = WorkoutRepository.ByExerciseId(exercise.Id.ToString()).Count
                    });
                }

                return list;
            }
            return null;
        }
    }
}
