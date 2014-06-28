using MvcTemplate.Objects;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Services
{
    public interface IAccountsService : IService
    {
        Boolean IsLoggedIn();
        Boolean AccountExists(String accountId);

        Boolean CanLogin(AccountLoginView account);
        Boolean CanRegister(AccountView account);
        Boolean CanEdit(ProfileEditView profile);
        Boolean CanEdit(AccountEditView account);
        Boolean CanDelete(AccountView profile);

        IEnumerable<AccountView> GetViews();
        TView GetView<TView>(String id) where TView : BaseView;

        void Register(AccountView account);
        void Edit(ProfileEditView profile);
        void Edit(AccountEditView view);
        void Delete(String accountId);

        void Login(String username);
        void Logout();
    }
}
