using System;
using Template.Objects;

namespace Template.Services
{
    public interface IAuthService : IService
    {
        Boolean IsLoggedIn();
        Boolean CanLogin(AuthView account);
        Boolean CanRegister(AuthView account);

        void Register(AuthView account);
        void Login(AuthView account);
        void Logout();
    }
}
