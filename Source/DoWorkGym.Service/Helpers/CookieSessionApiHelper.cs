using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DoWorkGym.Service.Helpers
{
    public class CookieSession
    {
        public static string CookieTokenCacheKeyPrefix
        {
            get { return "TA"; }
        }

        public string Token { get; set; }
        public string Theme { get; set; }

        public bool HasToken
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Token);
            }
        }

        public string CacheKeyToken
        {
            get
            {
                return (CookieTokenCacheKeyPrefix + Token);
            }
        }
    }


    public class CookieSessionApiHelper
    {
        public const string CookieHeaderName = "tasession";
        public const int CookieExpirationHours = 24;

        public const string CookieTokenName = "token";
        public const string CookieThemeName = "theme"; // for test only


        private CookieApiHelper _cookieApi;
        private CookieApiHelper CookieApi
        {
            get { return _cookieApi ?? (_cookieApi = new CookieApiHelper()); }
        }


        public CookieHeaderValue CreateCookieHeader(HttpRequestMessage request, string token)
        {
            var cookieExpire = DateTimeOffset.Now.AddHours(CookieExpirationHours);

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("token can not be null or empty!");
            }

            var values = new NameValueCollection();
            values[CookieTokenName] = token;
            values[CookieThemeName] = "testTheme.v4";

            CookieHeaderValue cookie = CookieApi.CreateCookie(request, CookieHeaderName, values, cookieExpire);

            return cookie;
        }


        private CookieHeaderValue GetCookieHeader(HttpRequestMessage request) //, string token = null
        {
            CookieHeaderValue cookie = CookieApi.GetCookie(request, CookieHeaderName);
            if (cookie == null)
            {
                return CreateCookieHeader(request, "no-cookie");
            }
            return cookie;
        }


        public CookieSession GetCookie(HttpRequestMessage request) //, string token = null
        {
            CookieSession cookieItem = null;

            CookieHeaderValue cookie = GetCookieHeader(request);
            var cookieState = cookie[CookieHeaderName];
            if (cookieState != null)
            {
                cookieItem = new CookieSession
                {
                    Token = cookieState[CookieTokenName],
                    Theme = cookieState[CookieThemeName]
                };
            }

            return cookieItem;
        }


        public CookieHeaderValue GetCookieAndExpire(HttpRequestMessage request)
        {
            return CookieApi.GetCookieAndExpire(request, CookieHeaderName);
        }
    }
}
