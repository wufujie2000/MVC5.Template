using System;
using Template.Objects;

namespace Template.Services
{
    public interface IAuthService : IService
    {
        Boolean IsLoggedIn();

        Boolean CanLogin(AccountLoginView account);
        Boolean CanRegister(AccountView account);

        void Register(AccountView account);
        void Login(AccountLoginView account);
        void Logout();
    }
}
