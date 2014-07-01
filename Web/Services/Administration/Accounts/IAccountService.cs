using MvcTemplate.Objects;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Services
{
    public interface IAccountService : IService
    {
        Boolean IsLoggedIn();
        Boolean AccountExists(String accountId);

        IEnumerable<AccountView> GetViews();
        TView GetView<TView>(String id) where TView : BaseView;

        void Register(AccountView view);
        void Edit(ProfileEditView view);
        void Edit(AccountEditView view);
        void Delete(String accountId);

        void Login(String username);
        void Logout();
    }
}
