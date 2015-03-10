using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using DoWorkGym.Infrastructure;
using DoWorkGym.Model;
using DoWorkGym.Service.Helpers;
using DoWorkGym.Util;

namespace DoWorkGym.Service
{
    public class AccountService
    {
        private CookieApiHelper _cookieApi;
        private CookieApiHelper CookieApi
        {
            get { return _cookieApi ?? (_cookieApi = new CookieApiHelper()); }
        }

        private CookieSessionApiHelper _cookieSessionApi;
        private CookieSessionApiHelper CookieSessionApi
        {
            get { return _cookieSessionApi ?? (_cookieSessionApi = new CookieSessionApiHelper()); }
        }

        private CookieUserInfoHelper _cookieUserInfoHelper;
        private CookieUserInfoHelper CookieUserInfoApi
        {
            get { return _cookieUserInfoHelper ?? (_cookieUserInfoHelper = new CookieUserInfoHelper()); }
        }

        private UserRepository _userRepository;
        private UserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository()); }
        }

        private MongoCache _mongoCache;
        private MongoCache MongoCache
        {
            get { return _mongoCache ?? (_mongoCache = new MongoCache()); }
        }


        // ///////////////////// PUBLIC METHODS /////////////////////

        public CookieHeaderValue CreateAccountAndLogin(string email, string password, HttpRequestMessage request)
        {
            User user = CreateUserAccount(email, password);

            string cookieToken = GenerateToken();
            string userId = user.Id.ToString();
            var cookieHeader = CreateCookieTokenCacheValue(userId, cookieToken, request);

            return cookieHeader;
        }


        public CookieHeaderValue Login(string email, string password, HttpRequestMessage request)
        {
            User user = LoginUser(email, password);

            string cookieToken = GenerateToken();
            string userId = user.Id.ToString();
            var cookieHeader = CreateCookieTokenCacheValue(userId, cookieToken, request);

            return cookieHeader;
        }


        public CookieHeaderValue SaveUserInfoCookie(HttpRequestMessage request, string email)
        {
            return CookieUserInfoApi.CreateCookieHeader(request, email);
        }


        public CookieHeaderValue Logout(HttpRequestMessage request)
        {
            var cookieObj = CookieSessionApi.GetCookie(request);

            // Clear Cache object.
            MongoCache.Clear(cookieObj.Token);

            // Expire cookie.
            var cookieHeader = CookieSessionApi.GetCookieAndExpire(request);

            Logging.Info(EventType.Logout, "User logged out.");

            return cookieHeader;
        }


        public CookieHeaderValue ExpireUserInfoCookie(HttpRequestMessage request)
        {
            // Expire cookie.
            var cookieHeader = CookieUserInfoApi.GetCookieAndExpire(request);
            return cookieHeader;
        }


        public User GetUserByEmail(string email)
        {
            return UserRepository.ByEmail(email);
        }


        public User ValidateUserPassword(User user, string password)
        {
            var userPasswordSalt = user.PasswordSalt;
            var userPassword = user.Password;
            if (HashHelper.ConfirmPassword(password, userPasswordSalt, userPassword))
            {
                return user;
            }

            return null;
        }


        public bool IsValidUserPassword(User user, string password)
        {
            if (ValidateUserPassword(user, password) != null)
            {
                return true;
            }
            return false;
        }


        public static bool IsAuthorized()
        {
            var cookieHelper = new CookieHelper();
            var cookieObj = cookieHelper.GetSessionCookie();

            if (cookieObj != null)
            {
                var cacheTokenKey = cookieObj.Token;

                var mongoCache = new MongoCache();

                string userId;
                if (mongoCache.Get(cacheTokenKey, out userId))
                {
                    return true;
                }
            }

            return false;
        }


        public static string AuthorizedUserEmail()
        {
            if (IsAuthorized())
            {
                var service = new AccountService();
                var user = service.GetAuthorizedUser();
                return user.Email;
            }

            return "-";
        }


        public User GetAuthorizedUser()
        {
            User user = null;

            var cookieHelper = new CookieHelper();
            var cookieObj = cookieHelper.GetSessionCookie();

            if (cookieObj != null)
            {
                var cacheTokenKey = cookieObj.Token;

                var mongoCache = new MongoCache();

                string userId;
                if (mongoCache.Get(cacheTokenKey, out userId))
                {
                    user = UserRepository.GetById(userId);
                    if (user == null)
                    {
                        Logging.Warn("User not found with id:" + userId);
                    }
                }
            }

            return user;
        }


        public HttpResponseMessage ResponseWithCookie(HttpResponseMessage response, CookieHeaderValue cookieHeader)
        {
            return CookieApi.ResponseWithCookie(response, cookieHeader);
        }


        public HttpResponseMessage ResponseWithCookie(HttpResponseMessage response, IEnumerable<CookieHeaderValue> cookieHeaders)
        {
            return CookieApi.ResponseWithCookie(response, cookieHeaders);
        }


        // ///////////////////// PRIVATE METHODS /////////////////////

        private string GenerateToken()
        {
            return (CookieSession.CookieTokenCacheKeyPrefix + Guid.NewGuid());
        }


        private CookieHeaderValue CreateCookieTokenCacheValue(string userId, string cookieToken, HttpRequestMessage request)
        {
            var cookieHeader = CookieSessionApi.CreateCookieHeader(request, cookieToken);

            const int cacheValidDays = 7;
            MongoCache.Add(cookieToken, userId, DateTime.Now.AddDays(cacheValidDays));

            return cookieHeader;
        }


        private User CreateUserAccount(string email, string password)
        {
            User user = UserRepository.ByEmail(email);
            if (user != null)
            {
                throw new ArgumentException("User with given e-mail already exists.");
            }

            string passwordSalt = HashHelper.GetPasswordSalt();
            var hashedPassword = HashHelper.GetHash(password, passwordSalt);

            user = new User
            {
                Email = email.ToLower(),
                Created = DateTime.Now,
                LastLogin = DateTime.Now,
                Password = hashedPassword,
                PasswordSalt = passwordSalt
            };

            UserRepository.Add(user);

            Logging.Info(EventType.NewUser, "User account created. " + email);

            return user;
        }


        private User LoginUser(string email, string password)
        {
            User user = UserRepository.ByEmail(email.ToLower());
            if (user == null)
            {
                // TODO: Create custom Exception class for this. (Not authorized)
                throw new ArgumentException("User with given e-mail does not exists. " + email);
            }

            if (!IsValidUserPassword(user, password))
            {
                // TODO: Create custom Exception class for this. (Not authorized)
                // then for Api return 'actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);'
                throw new ArgumentException("Invalid password given.");
            }

            UserRepository.UpdateLastLoginById(user.Id.ToString(), DateTime.Now);

            Logging.Info(EventType.Login, "User logged in. " + email);

            return user;
        }
    }
}
