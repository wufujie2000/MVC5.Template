using MvcTemplate.Objects;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Services
{
    public interface IAccountsService : IService
    {
        Boolean IsLoggedIn();
        Boolean AccountExists(String accountId);

        Boolean CanLogin(AccountLoginView view);
        Boolean CanRegister(AccountView view);
        Boolean CanEdit(ProfileEditView view);
        Boolean CanEdit(AccountEditView view);
        Boolean CanDelete(AccountView view);

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
