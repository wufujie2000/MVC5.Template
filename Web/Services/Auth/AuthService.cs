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
        public Boolean CanLogin(AuthView auth)
        {
            Boolean isValid = ModelState.IsValid;
            isValid &= IsAuthenticated(auth.Username, auth.Password);

            return isValid;
        }
        public Boolean CanRegister(AuthView account)
        {
            Boolean isValid = ModelState.IsValid;
            isValid &= IsUniqueUsername(account);
            isValid &= IsLegalPassword(account);

            isValid &= IsEmailSpecified(account);
            isValid &= IsUniqueEmail(account);

            return isValid;
        }

        public void AddSuccessfulRegistrationMessage()
        {
            AlertMessages.Add(AlertMessageType.Success, Messages.SuccesfulRegistration);
        }

        public void Register(AuthView account)
        {
            Account registration = UnitOfWork.ToModel<AuthView, Account>(account);
            registration.Passhash = hasher.HashPassword(account.Password);

            UnitOfWork.Repository<Account>().Insert(registration);
            UnitOfWork.Commit();
        }
        public void Login(AuthView account)
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

        private Boolean IsUniqueUsername(AuthView account)
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
        private Boolean IsLegalPassword(AuthView account)
        {
            Boolean isLegal = Regex.IsMatch(account.Password, "^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{8,}$");
            if (!isLegal)
                AlertMessages.AddError(Validations.IllegalPassword);

            return isLegal;
        }
        private Boolean IsEmailSpecified(AuthView account)
        {
            Boolean isSpecified = !String.IsNullOrEmpty(account.Email);

            if (!isSpecified)
                ModelState.AddModelError<AuthView>(model => model.Email, String.Empty);

            return isSpecified;
        }
        private Boolean IsUniqueEmail(AuthView account)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(acc => acc.Email.ToUpper() == account.Email.ToUpper())
                .Any();

            if (!isUnique)
                AlertMessages.AddError(Validations.EmailIsAlreadyUsed);

            return isUnique;
        }

        private void SetAccountId(AuthView account)
        {
            account.Id = UnitOfWork
                .Repository<Account>()
                .Query(acc => acc.Username.ToUpper() == account.Username.ToUpper())
                .First()
                .Id;
        }
        private void CreateCookieFor(AuthView account)
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
