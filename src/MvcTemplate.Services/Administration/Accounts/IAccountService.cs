using MvcTemplate.Objects;
using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace MvcTemplate.Services
{
    public interface IAccountService : IService
    {
        TView Get<TView>(String id) where TView : BaseView;
        IQueryable<AccountView> GetViews();

        Boolean AccountExists(String accountId);
        Boolean IsLoggedIn(IPrincipal user);

        Task Recover(AccountRecoveryView view, HttpRequestBase request);
        void Register(AccountRegisterView view);
        void Reset(AccountResetView view);

        void Create(AccountCreateView view);
        void Edit(ProfileEditView view);
        void Edit(AccountEditView view);
        void Delete(String id);

        void Login(String username);
        void Logout();
    }
}
