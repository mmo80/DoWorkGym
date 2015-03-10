using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DoWorkGym.Service.Helpers
{
    public class CookieApiHelper
    {
        public CookieHeaderValue CreateCookie(HttpRequestMessage request, string cookieHeaderName, NameValueCollection cookieValues, DateTimeOffset expire)
        {
            var cookie = new CookieHeaderValue(cookieHeaderName, cookieValues)
            {
                Expires = expire,
                Domain = request.RequestUri.Host,
                Path = "/"
            };

            return cookie;
        }


        public CookieHeaderValue CreateCookie(HttpRequestMessage request, string cookieHeaderName, string cookieValue, DateTimeOffset expire)
        {
            var cookie = new CookieHeaderValue(cookieHeaderName, cookieValue)
            {
                Expires = expire,
                Domain = request.RequestUri.Host,
                Path = "/"
            };

            return cookie;
        }


        public CookieHeaderValue GetCookie(HttpRequestMessage request, string cookieHeaderName)
        {
            CookieHeaderValue cookie = request.Headers.GetCookies(cookieHeaderName).FirstOrDefault();
            if (cookie != null)
            {
                // TODO: Update Expire time in cookie and cache??
                //CookieState cookieState = cookie[cookieHeaderName];
                return cookie;
            }
            return null;
        }


        public CookieHeaderValue GetCookieAndExpire(HttpRequestMessage request, string cookieHeaderName)
        {
            // Create cookie where date has expired!
            CookieHeaderValue cookie = CreateCookie(request, cookieHeaderName, string.Empty, DateTime.Now.AddDays(-1));
            return cookie;
        }


        public HttpResponseMessage ResponseWithCookie(HttpResponseMessage response, CookieHeaderValue cookieHeader)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response is null.");
            }

            response.Headers.AddCookies(new[] { cookieHeader });
            return response;
        }


        public HttpResponseMessage ResponseWithCookie(HttpResponseMessage response, IEnumerable<CookieHeaderValue> cookies)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response is null.");
            }

            response.Headers.AddCookies(cookies);
            return response;
        }
    }
}