using MvcTemplate.Components.Extensions.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using System;
using System.Linq;
using System.Web;

namespace MvcTemplate.Validators
{
    public class AccountValidator : BaseValidator, IAccountValidator
    {
        private IHasher hasher;

        public AccountValidator(IUnitOfWork unitOfWork, IHasher hasher)
            : base(unitOfWork)
        {
            this.hasher = hasher;
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

        private Boolean IsUniqueUsername(String accountId, String username)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Any(account =>
                    account.Id != accountId &&
                    account.Username.ToUpper() == username.ToUpper());

            if (!isUnique)
                ModelState.AddModelError<AccountView>(model => model.Username, Validations.UsernameIsAlreadyTaken);

            return isUnique;
        }
        private Boolean IsUniqueEmail(String accountId, String email)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Any(account =>
                    account.Id != accountId &&
                    account.Email.ToUpper() == email.ToUpper());

            if (!isUnique)
                ModelState.AddModelError<AccountView>(model => model.Email, Validations.EmailIsAlreadyUsed);

            return isUnique;
        }

        private Boolean IsAuthenticated(String username, String password)
        {
            String passhash = UnitOfWork
                .Repository<Account>()
                .Where(acc => acc.Username.ToUpper() == username.ToUpper())
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
                .Where(account => account.Id == HttpContext.Current.User.Identity.Name)
                .Select(account => account.Passhash)
                .First();

            Boolean isCorrectPassword = passhash != null && hasher.Verify(password, passhash);
            if (!isCorrectPassword)
                ModelState.AddModelError<ProfileEditView>(model => model.Password, Validations.IncorrectPassword);

            return isCorrectPassword;
        }
    }
}
