using System.Web;

namespace DoWorkGym.Service.Helpers
{
    public class CookieHelper
    {
        public CookieSession GetSessionCookie()
        {
            var cookieItem = new CookieSession();

            var cookieCollection = HttpContext.Current.Request.Cookies[CookieSessionApiHelper.CookieHeaderName];
            if (cookieCollection != null)
            {
                // TODO: Update Expire time in cookie and cache??
                cookieItem.Token = cookieCollection[CookieSessionApiHelper.CookieTokenName];
                cookieItem.Theme = cookieCollection[CookieSessionApiHelper.CookieThemeName];
            }

            return cookieItem;
        }
    }
}
