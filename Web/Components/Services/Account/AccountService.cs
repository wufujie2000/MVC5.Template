using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Template.Components.Security;
using Template.Objects;
using Template.Resources.Views.AccountView;

namespace Template.Components.Services
{
    public class AccountService : GenericService<Account, AccountView>
    {
        public AccountService(ModelStateDictionary modelState)
            : base(modelState)
        {
        }

        public Boolean CanLogin(AccountView accountView)
        {
            Boolean isValid = ModelState.IsValid;
            isValid &= IsAuthenticated(accountView.Username, accountView.Password);

            return isValid;
        }
        
        public void Login(AccountView account)
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
                .Query(acc => acc.Username == username)
                .SingleOrDefault();

            return AccountExists(account) && IsCorrectPassword(password, account.Passhash);
        }
        private Boolean AccountExists(Account account)
        {
            if (account == null)
                ModelState.AddModelError(String.Empty, Validations.IncorrectUsernameOrPassword);

            return account != null;
        }
        private Boolean IsCorrectPassword(String password, String passhash)
        {
            Boolean passwordCorrect = BCrypter.Verify(password, passhash);
            if (!passwordCorrect)
                ModelState.AddModelError(String.Empty, Validations.IncorrectUsernameOrPassword);

            return passwordCorrect;
        }

        private void SetAccountId(AccountView account)
        {
            account.Id = UnitOfWork
                .Repository<Account>()
                .Query(acc => acc.Username == account.Username)
                .First()
                .Id;
        }
        private void CreateCookieFor(AccountView account)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, account.Id, DateTime.Now, DateTime.Now.AddMonths(1), true, account.Id);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true,
                Expires = ticket.Expiration
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
