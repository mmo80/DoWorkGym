using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DoWorkGym.Service;
using DoWorkGym.WebApp.ViewModels.AccountViewModels;

namespace DoWorkGym.WebApp.Controllers.Api
{
    public class AccountController : ApiController
    {
        private AccountService _accountService;
        private AccountService AccountService
        {
            get { return _accountService ?? (_accountService = new AccountService()); }
        }


        public HttpResponseMessage Register(RegisterUser registerUser)
        {
            var cookieHeader = AccountService.CreateAccountAndLogin(registerUser.Email, registerUser.Password, Request);
            return AccountService.ResponseWithCookie(Request.CreateResponse(HttpStatusCode.OK), cookieHeader);
        }


        public HttpResponseMessage Login(LoginUser loginUser)
        {
            var cookieHeaders = new List<CookieHeaderValue>();

            var sessionCookieHeader = AccountService.Login(loginUser.Email, loginUser.Password, Request);
            cookieHeaders.Add(sessionCookieHeader);

            if (loginUser.RememberMe)
            {
                var userInfoCookieHeader = AccountService.SaveUserInfoCookie(Request, loginUser.Email);
                cookieHeaders.Add(userInfoCookieHeader);
            }
            else
            {
                // Expire cookie so it is removed!
                var userInfoCookieHeader = AccountService.ExpireUserInfoCookie(Request);
                cookieHeaders.Add(userInfoCookieHeader);
            }

            return AccountService.ResponseWithCookie(Request.CreateResponse(HttpStatusCode.OK), cookieHeaders);
        }


        public HttpResponseMessage Logout()
        {
            var cookieHeader = AccountService.Logout(Request);
            return AccountService.ResponseWithCookie(Request.CreateResponse(HttpStatusCode.OK), cookieHeader);
        }
    }
}
