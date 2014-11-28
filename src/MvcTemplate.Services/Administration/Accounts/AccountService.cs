using MvcTemplate.Components.Mail;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcTemplate.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private IMailClient mailClient;
        private IHasher hasher;

        public AccountService(IUnitOfWork unitOfWork, IMailClient mailClient, IHasher hasher)
            : base(unitOfWork)
        {
            this.mailClient = mailClient;
            this.hasher = hasher;
        }

        public Boolean IsLoggedIn()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        public Boolean AccountExists(String accountId)
        {
            return UnitOfWork.Repository<Account>().Any(account => account.Id == accountId);
        }

        public IEnumerable<AccountView> GetViews()
        {
            return UnitOfWork
                .Repository<Account>()
                .To<AccountView>()
                .OrderByDescending(view => view.CreationDate);
        }
        public TView GetView<TView>(String id) where TView : BaseView
        {
            return UnitOfWork.Repository<Account>().GetById<TView>(id);
        }

        public void Recover(AccountRecoveryView view)
        {
            Account account = UnitOfWork.Repository<Account>().SingleOrDefault(acc => acc.Email.ToLower() == view.Email.ToLower());
            if (account == null) return;

            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(30);
            account.RecoveryToken = Guid.NewGuid().ToString();

            UnitOfWork.Repository<Account>().Update(account);
            UnitOfWork.Commit();

            HttpRequest request = HttpContext.Current.Request;
            UrlHelper urlHelper = new UrlHelper(request.RequestContext);
            String url = urlHelper.Action("Reset", "Auth", new { token = account.RecoveryToken }, request.Url.Scheme);
            String recoveryEmailBody = String.Format(Messages.RecoveryEmailBody, url);

            mailClient.Send(account.Email, Messages.RecoveryEmailSubject, recoveryEmailBody);
        }
        public void Reset(AccountResetView view)
        {
            Account account = UnitOfWork.Repository<Account>().Single(acc => acc.RecoveryToken == view.Token);
            account.Passhash = hasher.HashPassword(view.NewPassword);
            account.RecoveryTokenExpirationDate = null;
            account.RecoveryToken = null;

            UnitOfWork.Repository<Account>().Update(account);
            UnitOfWork.Commit();
        }
        public void Register(AccountView view)
        {
            Account account = UnitOfWork.To<Account>(view);
            view.Email = account.Email = view.Email.ToLower();
            account.Passhash = hasher.HashPassword(view.Password);

            UnitOfWork.Repository<Account>().Insert(account);
            UnitOfWork.Commit();
        }
        public void Edit(ProfileEditView view)
        {
            Account account = UnitOfWork.Repository<Account>().GetById(HttpContext.Current.User.Identity.Name);
            view.Email = account.Email = view.Email.ToLower();
            account.Username = view.Username;

            if (!String.IsNullOrWhiteSpace(view.NewPassword))
                account.Passhash = hasher.HashPassword(view.NewPassword);

            UnitOfWork.Repository<Account>().Update(account);
            UnitOfWork.Commit();
        }
        public void Edit(AccountEditView view)
        {
            Account account = UnitOfWork.Repository<Account>().GetById(view.Id);
            account.RoleId = view.RoleId;

            UnitOfWork.Repository<Account>().Update(account);
            UnitOfWork.Commit();

            Authorization.Provider.Refresh();
        }
        public void Delete(String id)
        {
            UnitOfWork.Repository<Account>().Delete(id);
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
                .Repository<Account>()
                .Where(account => account.Username.ToLower() == username.ToLower())
                .Select(account => account.Id)
                .Single();
        }
    }
}
