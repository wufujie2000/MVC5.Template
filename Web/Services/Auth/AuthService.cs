using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Template.Components.Alerts;
using Template.Components.Extensions.Mvc;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.AccountView;

namespace Template.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private IHasher hasher;

        public AuthService(IUnitOfWork unitOfWork, IHasher hasher)
            : base(unitOfWork)
        {
            this.hasher = hasher;
        }

        public Boolean IsLoggedIn()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        public Boolean CanLogin(AccountLoginView auth)
        {
            Boolean isValid = ModelState.IsValid;
            isValid &= IsAuthenticated(auth.Username, auth.Password);

            return isValid;
        }
        public Boolean CanRegister(AccountView account)
        {
            Boolean isValid = ModelState.IsValid;
            isValid &= IsUniqueUsername(account);
            isValid &= IsLegalPassword(account);

            isValid &= IsEmailSpecified(account);
            isValid &= IsUniqueEmail(account);

            return isValid;
        }

        public void Register(AccountView account)
        {
            Account registration = UnitOfWork.ToModel<AccountView, Account>(account);
            registration.Passhash = hasher.HashPassword(account.Password);

            UnitOfWork.Repository<Account>().Insert(registration);
            UnitOfWork.Commit();
        }
        public void Login(AccountLoginView account)
        {
            SetAccountId(account);
            CreateCookieFor(account);
        }
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        private Boolean IsAuthenticated(String username, String password)
        {
            Account account = UnitOfWork
                .Repository<Account>()
                .Query(acc => acc.Username.ToUpper() == username.ToUpper())
                .SingleOrDefault();

            return AccountExists(account) && IsCorrectPassword(password, account.Passhash);
        }
        private Boolean AccountExists(Account account)
        {
            if (account == null)
                AlertMessages.AddError(Validations.IncorrectUsernameOrPassword);

            return account != null;
        }
        private Boolean IsCorrectPassword(String password, String passhash)
        {
            Boolean passwordCorrect = hasher.Verify(password, passhash);
            if (!passwordCorrect)
                AlertMessages.AddError(Validations.IncorrectUsernameOrPassword);

            return passwordCorrect;
        }

        private Boolean IsUniqueUsername(AccountView account)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(acc =>
                    acc.Username.ToUpper() == account.Username.ToUpper())
                .Any();

            if (!isUnique)
                AlertMessages.AddError(Validations.UsernameIsAlreadyTaken);

            return isUnique;
        }
        private Boolean IsLegalPassword(AccountView account)
        {
            Boolean isLegal = Regex.IsMatch(account.Password, "^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{8,}$");
            if (!isLegal)
                AlertMessages.AddError(Validations.IllegalPassword);

            return isLegal;
        }
        private Boolean IsEmailSpecified(AccountView account)
        {
            Boolean isSpecified = !String.IsNullOrEmpty(account.Email);

            if (!isSpecified)
                ModelState.AddModelError<AccountView>(model => model.Email, String.Empty);

            return isSpecified;
        }
        private Boolean IsUniqueEmail(AccountView account)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(acc => acc.Email.ToUpper() == account.Email.ToUpper())
                .Any();

            if (!isUnique)
                AlertMessages.AddError(Validations.EmailIsAlreadyUsed);

            return isUnique;
        }

        private void SetAccountId(AccountLoginView account)
        {
            account.Id = UnitOfWork
                .Repository<Account>()
                .Query(acc => acc.Username.ToUpper() == account.Username.ToUpper())
                .First()
                .Id;
        }
        private void CreateCookieFor(AccountLoginView account)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, account.Id, DateTime.Now, DateTime.Now.AddMonths(1), true, account.Id);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                Expires = ticket.Expiration,
                HttpOnly = true
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
