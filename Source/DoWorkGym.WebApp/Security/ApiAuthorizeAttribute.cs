using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DoWorkGym.Service.Helpers;
using DoWorkGym.Util;

namespace DoWorkGym.WebApp.Security
{
    public class ApiAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        private CookieSessionApiHelper _cookieSessionApi;
        private CookieSessionApiHelper CookieSessionApi
        {
            get { return _cookieSessionApi ?? (_cookieSessionApi = new CookieSessionApiHelper()); }
        }

        private MongoCache _mongoCache;
        private MongoCache MongoCache
        {
            get { return _mongoCache ?? (_mongoCache = new MongoCache()); }
        }


        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext is null");
            }

            if (!actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                // Get cookie
                var cookieSession = CookieSessionApi.GetCookie(actionContext.Request);
                string cacheTokenKey = cookieSession.Token;

                // Url
                string pathUrl = GetUrlPath(actionContext);

                string userId;
                if (MongoCache.Get(cacheTokenKey, out userId))
                {
                    // TODO: Verify that user exists!
                    Logging.Info(string.Format("UserId({0}) found in cache with given token({1}). {2}", userId, cacheTokenKey, pathUrl));
                }
                else
                {
                    Logging.Warn(string.Format("No item found with given token({0}). {1}", cacheTokenKey, pathUrl));
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
        }


        public string GetUrlPath(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string path = "-";

            if (actionContext != null)
            {
                if (actionContext.Request != null && actionContext.Request.RequestUri != null)
                {
                    if (!string.IsNullOrWhiteSpace(actionContext.Request.RequestUri.AbsolutePath))
                    {
                        path = actionContext.Request.RequestUri.AbsolutePath;
                    }
                }
            }

            return path;
        }
    }
}