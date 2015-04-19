using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Security;

namespace MvcTemplate.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private IHasher hasher;

        public AccountService(IUnitOfWork unitOfWork, IHasher hasher)
            : base(unitOfWork)
        {
            this.hasher = hasher;
        }

        public TView Get<TView>(String id) where TView : BaseView
        {
            return UnitOfWork.GetAs<Account, TView>(id);
        }
        public IQueryable<AccountView> GetViews()
        {
            return UnitOfWork
                .Select<Account>()
                .To<AccountView>()
                .OrderByDescending(account => account.CreationDate);
        }

        public Boolean AccountExists(String accountId)
        {
            return UnitOfWork.Select<Account>().Any(account => account.Id == accountId);
        }
        public Boolean IsLoggedIn(IPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }

        public String Recover(AccountRecoveryView view)
        {
            Account account = UnitOfWork.Select<Account>().SingleOrDefault(acc => acc.Email.ToLower() == view.Email.ToLower());
            if (account == null) return null;

            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(30);
            account.RecoveryToken = Guid.NewGuid().ToString();

            UnitOfWork.Update(account);
            UnitOfWork.Commit();

            return account.RecoveryToken;
        }
        public void Register(AccountRegisterView view)
        {
            Account account = UnitOfWork.To<Account>(view);
            view.Email = account.Email = view.Email.ToLower();
            account.Passhash = hasher.HashPassword(view.Password);

            UnitOfWork.Insert(account);
            UnitOfWork.Commit();
        }
        public void Reset(AccountResetView view)
        {
            Account account = UnitOfWork.Select<Account>().Single(acc => acc.RecoveryToken == view.Token);
            account.Passhash = hasher.HashPassword(view.NewPassword);
            account.RecoveryTokenExpirationDate = null;
            account.RecoveryToken = null;

            UnitOfWork.Update(account);
            UnitOfWork.Commit();
        }

        public void Create(AccountCreateView view)
        {
            Account account = UnitOfWork.To<Account>(view);
            view.Email = account.Email = view.Email.ToLower();
            account.Passhash = hasher.HashPassword(view.Password);

            UnitOfWork.Insert(account);
            UnitOfWork.Commit();
        }
        public void Edit(ProfileEditView view)
        {
            Account account = UnitOfWork.Get<Account>(view.Id);
            view.Email = account.Email = view.Email.ToLower();
            account.Username = view.Username;

            if (!String.IsNullOrWhiteSpace(view.NewPassword))
                account.Passhash = hasher.HashPassword(view.NewPassword);

            UnitOfWork.Update(account);
            UnitOfWork.Commit();
        }
        public void Edit(AccountEditView view)
        {
            Account account = UnitOfWork.Get<Account>(view.Id);
            account.RoleId = view.RoleId;

            UnitOfWork.Update(account);
            UnitOfWork.Commit();

            Authorization.Provider.Refresh();
        }
        public void Delete(String id)
        {
            UnitOfWork.Delete<Account>(id);
            UnitOfWork.Commit();
        }

        public void Login(String username)
        {
            FormsAuthentication.SetAuthCookie(GetAccountId(username), true);
        }
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        private String GetAccountId(String username)
        {
            return UnitOfWork
                .Select<Account>()
                .Where(account => account.Username.ToLower() == username.ToLower())
                .Select(account => account.Id)
                .Single();
        }
    }
}
