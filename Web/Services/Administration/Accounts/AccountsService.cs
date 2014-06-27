using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Template.Components.Extensions.Mvc;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.AccountView;

namespace Template.Services
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

        public Boolean CanLogin(AccountLoginView login)
        {
            Boolean isValid = IsAuthenticated(login.Username, login.Password);
            isValid &= ModelState.IsValid;

            return isValid;
        }
        public Boolean CanRegister(AccountView account)
        {
            Boolean isValid = IsUniqueUsername(account.Id, account.Username);
            isValid &= IsUniqueEmail(account.Id, account.Email);
            isValid &= ModelState.IsValid;

            return isValid;
        }
        public Boolean CanEdit(ProfileEditView profile)
        {
            Boolean isValid = IsUniqueUsername(HttpContext.Current.User.Identity.Name, profile.Username);
            isValid &= IsUniqueEmail(HttpContext.Current.User.Identity.Name, profile.Email);
            isValid &= IsCorrectPassword(profile.Password);
            isValid &= ModelState.IsValid;

            return isValid;
        }
        public Boolean CanEdit(AccountEditView account)
        {
            return ModelState.IsValid;
        }
        public Boolean CanDelete(AccountView account)
        {
            return IsCorrectPassword(account.Password);
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

        public void Register(AccountView account)
        {
            Account registration = UnitOfWork.ToModel<AccountView, Account>(account);
            registration.Passhash = hasher.HashPassword(account.Password);

            UnitOfWork.Repository<Account>().Insert(registration);
            UnitOfWork.Commit();
        }
        public void Edit(ProfileEditView profile)
        {
            Account account = UnitOfWork.Repository<Account>().GetById(HttpContext.Current.User.Identity.Name);
            account.Username = profile.Username;
            account.Email = profile.Email;

            if (!String.IsNullOrWhiteSpace(profile.NewPassword))
                account.Passhash = hasher.HashPassword(profile.NewPassword);

            UnitOfWork.Repository<Account>().Update(account);
            UnitOfWork.Commit();
        }
        public void Edit(AccountEditView view)
        {
            Account accountInDatabase = UnitOfWork.Repository<Account>().GetById(view.Id);
            Account account = UnitOfWork.ToModel<AccountEditView, Account>(view);

            account.Username = accountInDatabase.Username;
            account.Passhash = accountInDatabase.Passhash;
            account.Email = accountInDatabase.Email;

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
                AlertMessages.AddError(Validations.IncorrectUsernameOrPassword);

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
