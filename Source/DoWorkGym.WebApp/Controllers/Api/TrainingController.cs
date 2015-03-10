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

namespace DoWorkGym.WebApp.Controllers.Api
{
    [ApiAuthorize]
    public class TrainingController : ApiController
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


        public HttpResponseMessage AddTraining(AddTrainingView trainingView)
        {
            User user = AccountService.GetAuthorizedUser();
            var training = new Training()
            {
                Name = trainingView.Name,
                UserId = user.Id
            };

            TrainingRepository.AddTraining(training);
            return Request.CreateResponse(HttpStatusCode.Created);
        }


        public HttpResponseMessage DeleteTraining(TrainingView trainingView)
        {
            TrainingRepository.DeleteTraining(trainingView.Id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
