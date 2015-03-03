using MvcTemplate.Objects;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MvcTemplate.Services
{
    public interface IAccountService : IService
    {
        TView Get<TView>(String id) where TView : BaseView;
        IQueryable<AccountView> GetViews();

        Boolean AccountExists(String accountId);
        Boolean IsLoggedIn(IPrincipal user);

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
