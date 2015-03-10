using System.Web.Mvc;
using DoWorkGym.Service;
using DoWorkGym.WebApp.ViewModels.WorkoutViewModels;

namespace DoWorkGym.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult CheckAuthorization(ActionResult returnView)
        {
            if (!AccountService.IsAuthorized())
            {
                return RedirectToAction("NotAuthorized");
            }
            return returnView;
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Register()
        {
            return View();
        }


        public ActionResult Training()
        {
            return CheckAuthorization(View());
        }


        public ActionResult Exercise()
        {
            return CheckAuthorization(View());
        }


        public ActionResult Workout()
        {
            return CheckAuthorization(View());
        }


        public ActionResult Error()
        {
            return View();
        }


        public ActionResult NotAuthorized()
        {
            return View();
        }


        public ActionResult WorkoutDetail(string trainingId, string exerciseId)
        {
            ViewBag.TrainingId = trainingId;
            ViewBag.ExerciseId = exerciseId;

            var trainingService = new TrainingService();
            var list = trainingService.GetTrainingAndExerciseNames(trainingId, exerciseId);

            var view = new TrainingExerciseView();

            if (list != null)
            {
                view = new TrainingExerciseView
                {
                    Training = list[trainingId],
                    Exercise = list[exerciseId]
                };
            }

            return CheckAuthorization(View(view));
        }


        public ActionResult History()
        {
            return CheckAuthorization(View());
        }
    }
}
