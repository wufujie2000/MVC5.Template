using MvcTemplate.Objects;
using System;
using System.Linq;

namespace MvcTemplate.Services
{
    public interface IAccountService : IService
    {
        Boolean IsLoggedIn();
        Boolean AccountExists(String accountId);

        IQueryable<AccountView> GetViews();
        TView Get<TView>(String id) where TView : BaseView;

        void Recover(AccountRecoveryView view);
        void Reset(AccountResetView view);
        void Register(AccountView view);
        void Edit(ProfileEditView view);
        void Edit(AccountEditView view);
        void Delete(String accountId);

        void Login(String username);
        void Logout();
    }
}
