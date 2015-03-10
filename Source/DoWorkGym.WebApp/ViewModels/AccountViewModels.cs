
namespace DoWorkGym.WebApp.ViewModels
{
    namespace AccountViewModels
    {
        public class RegisterUser
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class LoginUser
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }
    }
}