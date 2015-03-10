using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.UI.WebControls.Expressions;
using DoWorkGym.Infrastructure;
using DoWorkGym.Model;
using DoWorkGym.Service;
using DoWorkGym.WebApp.ViewModels.HistoryViewModels;

namespace DoWorkGym.WebApp.Controllers.Api
{
    public class HistoryController : ApiController
    {
        private AccountService _accountService;
        private AccountService AccountService
        {
            get { return _accountService ?? (_accountService = new AccountService()); }
        }

        private WorkoutRepository _workoutRepository;
        private WorkoutRepository WorkoutRepository
        {
            get { return _workoutRepository ?? (_workoutRepository = new WorkoutRepository()); }
        }

        private TrainingRepository _trainingRepository;
        private TrainingRepository TrainingRepository
        {
            get { return _trainingRepository ?? (_trainingRepository = new TrainingRepository()); }
        }


        public List<HistoryListView> GetHistory()
        {
            User user = AccountService.GetAuthorizedUser();

            var list = WorkoutRepository.ByUser(user.Id);

            var query = from w in list
                        group w by new
                        {
                            w.Exercise.Id,
                            w.Date
                        }
                        into o
                        orderby o.Key.Date descending
                        select new
                        {
                            Date = o.Key.Date,
                            Children = o.ToList()
                        };

            var prevDate = new DateTime();
            var setList = new List<string>();
            var returnList = new List<HistoryListView>();
            var item = new HistoryListView();

            foreach (var x in query)
            {
                if (!prevDate.Equals(x.Date))
                {
                    item = new HistoryListView()
                    {
                        Date = x.Date.ToString("yyyy-MM-dd"),
                        DateMillisecs = x.Date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds.ToString(),
                        DateRows = new List<HistoryItemDateRow>()
                    };

                    returnList.Add(item);
                }

                var children = x.Children.OrderBy(c => c.Set);
                foreach (var workout in children)
                {
                    setList.Add(string.Format("{0}: {1}x{2}{3}", workout.Set, workout.Reps, workout.Weight, workout.Unit));
                }

                var commaSeparated = string.Join(", ", setList);

                var exerciseName = "(exercise: not found)";
                var trainingName = "(training: not found)";
                if (x.Children.Any())
                {
                    if (x.Children.First().Exercise != null)
                    {
                        var exercise = x.Children.First().Exercise;
                        exerciseName = exercise.Name;

                        var training = TrainingRepository.GetByExerciseId(exercise.Id);
                        if (training != null)
                        {
                            trainingName = training.Name;
                        }
                    }
                }

                item.DateRows.Add(new HistoryItemDateRow()
                {
                    Exercise = string.Format("{0}:: {1} ({2})", trainingName, exerciseName, commaSeparated),
                    Workout = "-"
                });

                setList = new List<string>();
                prevDate = x.Date;
            }

            return returnList;
        }



        public RootObject GetHistoryCalendar()
        {
            User user = AccountService.GetAuthorizedUser();

            var list = WorkoutRepository.ByUser(user.Id);

            var query = from w in list
                orderby w.Date descending, w.Set descending 
                select w;

            var returnObj = new RootObject();
            returnObj.Success = 1;
            returnObj.Result = new List<Result>();
            string trainingName = null;

            foreach (var workout in query)
            {
                var dateStart = workout.Date.AddMinutes(workout.Set).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
                var dateEnd = workout.Date.AddMinutes(workout.Set + 1).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));

                var training = TrainingRepository.GetByExerciseId(workout.Exercise.Id);
                if (training != null)
                {
                    trainingName = training.Name;
                }

                var result = new Result()
                {
                    @class = "event-success",
                    start = dateStart.TotalMilliseconds.ToString(),
                    end = dateEnd.TotalMilliseconds.ToString(),
                    title = string.Format("{6}-{4}: #{0} {1} @ {2} {3} ({5})", workout.Set, workout.Reps, workout.Weight, workout.Unit, workout.Exercise.Name, workout.Note, trainingName),
                    id = workout.Id.ToString()//,
                    //url = "http://www.sprofile.se"
                };

                returnObj.Result.Add(result);
            }

            return returnObj;
        }
    }
}
