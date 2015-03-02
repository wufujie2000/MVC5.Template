using MvcTemplate.Objects;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MvcTemplate.Services
{
    public interface IAccountService : IService
    {
        Boolean IsLoggedIn(IPrincipal user);
        Boolean AccountExists(String accountId);

        IQueryable<AccountView> GetViews();
        TView Get<TView>(String id) where TView : BaseView;

        void Recover(AccountRecoveryView view, HttpRequestBase request);
        void Reset(AccountResetView view);
        void Register(AccountView view);
        void Edit(ProfileEditView view);
        void Edit(AccountEditView view);
        void Delete(String accountId);

        void Login(String username);
        void Logout();
    }
}
