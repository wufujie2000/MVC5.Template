using System;
using Template.Objects;

namespace Template.Services
{
    public interface IAuthService : IService
    {
        Boolean IsLoggedIn();
        Boolean CanLogin(LoginView account);

        void Login(LoginView account);
        void Logout();
    }
}
