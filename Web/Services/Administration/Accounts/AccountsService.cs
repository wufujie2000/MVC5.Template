using MvcTemplate.Components.Extensions.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MvcTemplate.Services
{
    public class AccountsService : BaseService, IAccountsService
    {
        private IHasher hasher;

        public AccountsService(IUnitOfWork unitOfWork, IHasher hasher)
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

        public Boolean CanLogin(AccountLoginView view)
        {
            Boolean isValid = IsAuthenticated(view.Username, view.Password);
            isValid &= ModelState.IsValid;

            return isValid;
        }
        public Boolean CanRegister(AccountView view)
        {
            Boolean isValid = IsUniqueUsername(view.Id, view.Username);
            isValid &= IsUniqueEmail(view.Id, view.Email);
            isValid &= ModelState.IsValid;

            return isValid;
        }
        public Boolean CanEdit(ProfileEditView view)
        {
            Boolean isValid = IsUniqueUsername(HttpContext.Current.User.Identity.Name, view.Username);
            isValid &= IsUniqueEmail(HttpContext.Current.User.Identity.Name, view.Email);
            isValid &= IsCorrectPassword(view.Password);
            isValid &= ModelState.IsValid;

            return isValid;
        }
        public Boolean CanEdit(AccountEditView view)
        {
            return ModelState.IsValid;
        }
        public Boolean CanDelete(AccountView view)
        {
            return IsCorrectPassword(view.Password);
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

        private Boolean IsUniqueUsername(String accountId, String username)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id != accountId &&
                    account.Username.ToUpper() == username.ToUpper())
                .Any();

            if (!isUnique)
                ModelState.AddModelError<AccountView>(model => model.Username, Validations.UsernameIsAlreadyTaken);

            return isUnique;
        }
        private Boolean IsUniqueEmail(String accountId, String email)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id != accountId &&
                    account.Email.ToUpper() == email.ToUpper())
                .Any();

            if (!isUnique)
                ModelState.AddModelError<AccountView>(model => model.Email, Validations.EmailIsAlreadyUsed);

            return isUnique;
        }

        private Boolean IsAuthenticated(String username, String password)
        {
            String passhash = UnitOfWork
                .Repository<Account>()
                .Query(acc => acc.Username.ToUpper() == username.ToUpper())
                .Select(acc => acc.Passhash)
                .SingleOrDefault();

            return IsCorrectPassword(password, passhash);
        }
        private Boolean IsCorrectPassword(String password, String passhash)
        {
            Boolean passwordCorrect = passhash != null && hasher.Verify(password, passhash);
            if (!passwordCorrect)
                ModelState.AddModelError(String.Empty, Validations.IncorrectUsernameOrPassword);

            return passwordCorrect;
        }
        private Boolean IsCorrectPassword(String password)
        {
            String passhash = UnitOfWork
                .Repository<Account>()
                .Query(account => account.Id == HttpContext.Current.User.Identity.Name)
                .Select(account => account.Passhash)
                .First();

            Boolean isCorrectPassword = passhash != null && hasher.Verify(password, passhash);
            if (!isCorrectPassword)
                ModelState.AddModelError<ProfileEditView>(model => model.Password, Validations.IncorrectPassword);

            return isCorrectPassword;
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
