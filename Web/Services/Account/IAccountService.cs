using System;
using Template.Objects;

namespace Template.Services
{
    public interface IAccountService : IService
    {
        Boolean IsLoggedIn();
        Boolean CanLogin(AccountView account);

        void Login(AccountView account);
        void Logout();
    }
}
