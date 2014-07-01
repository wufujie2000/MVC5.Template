using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public Boolean IsLoggedIn()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        public Boolean AccountExists(String accountId)
        {
            return UnitOfWork.Repository<Account>().Query(account => account.Id == accountId).Any();
        }

        public IEnumerable<AccountView> GetViews()
        {
            return UnitOfWork
                .Repository<Account>()
                .Query<AccountView>()
                .OrderByDescending(view => view.EntityDate);
        }
        public TView GetView<TView>(String id) where TView : BaseView
        {
            return UnitOfWork.Repository<Account>().GetById<TView>(id);
        }

        public void Register(AccountView view)
        {
            Account account = UnitOfWork.ToModel<AccountView, Account>(view);
            account.Passhash = hasher.HashPassword(view.Password);

            UnitOfWork.Repository<Account>().Insert(account);
            UnitOfWork.Commit();
        }
        public void Edit(ProfileEditView view)
        {
            Account account = UnitOfWork.Repository<Account>().GetById(HttpContext.Current.User.Identity.Name);
            account.Username = view.Username;
            account.Email = view.Email;

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
        }
        public void Delete(String id)
        {
            UnitOfWork.Repository<Account>().Delete(id);
            UnitOfWork.Commit();
        }

        public void Login(String username)
        {
            CreateCookieFor(GetAccountId(username));
        }
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        private String GetAccountId(String username)
        {
            return UnitOfWork
                .Repository<Account>()
                .Query(acc => acc.Username.ToUpper() == username.ToUpper())
                .First()
                .Id;
        }
        private void CreateCookieFor(String accountId)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, accountId, DateTime.Now, DateTime.Now.AddMonths(1), true, accountId);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                Expires = ticket.Expiration,
                HttpOnly = true
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
