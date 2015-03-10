using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DoWorkGym.Service.Helpers
{
    public class CookieUserInfoHelper
    {
        private const string CookieHeaderName = "dowork";


        private CookieApiHelper _cookieApi;
        private CookieApiHelper CookieApi
        {
            get { return _cookieApi ?? (_cookieApi = new CookieApiHelper()); }
        }


        public CookieHeaderValue CreateCookieHeader(HttpRequestMessage request, string email)
        {
            var cookieExpire = DateTimeOffset.Now.AddYears(1);

            //if (string.IsNullOrEmpty(email))
            //{
            //    throw new ArgumentNullException("email can not be null or empty!");
            //}

            string userValuesJson = Newtonsoft.Json.JsonConvert.SerializeObject(new {em = email});
            CookieHeaderValue cookie = CookieApi.CreateCookie(request, CookieHeaderName, userValuesJson, cookieExpire);

            return cookie;
        }


        public CookieHeaderValue GetCookieAndExpire(HttpRequestMessage request)
        {
            return CookieApi.GetCookieAndExpire(request, CookieHeaderName);
        }

        
        //public CookieHeaderValue GetCookieHeader(HttpRequestMessage request, string email)
        //{
        //    CookieHeaderValue cookie = CookieApi.GetCookie(request, CookieHeaderNameUserInfo);
        //    if (cookie == null)
        //    {
        //        return CreateCookieHeader(request, email);
        //    }

        //    return cookie;
        //}
    }
}
