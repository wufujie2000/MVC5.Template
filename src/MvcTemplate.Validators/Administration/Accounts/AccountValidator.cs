using MvcTemplate.Components.Extensions.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using System;
using System.Linq;

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

        public Boolean CanRecover(AccountRecoveryView view)
        {
            return ModelState.IsValid;
        }
        public Boolean CanReset(AccountResetView view)
        {
            Boolean isValid = IsValidResetToken(view.Token);
            isValid &= ModelState.IsValid;

            return isValid;
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
            Boolean isValid = IsUniqueUsername(view.Id, view.Username);
            isValid &= IsCorrectPassword(view.Id, view.Password);
            isValid &= IsUniqueEmail(view.Id, view.Email);
            isValid &= ModelState.IsValid;

            return isValid;
        }
        public Boolean CanEdit(AccountEditView view)
        {
            return ModelState.IsValid;
        }
        public Boolean CanDelete(ProfileDeleteView view)
        {
            Boolean isValid = IsCorrectPassword(view.Id, view.Password);
            isValid &= ModelState.IsValid;

            return isValid;
        }

        private Boolean IsUniqueUsername(String accountId, String username)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Any(account =>
                    account.Id != accountId &&
                    account.Username.ToLower() == username.ToLower());

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
                    account.Email.ToLower() == email.ToLower());

            if (!isUnique)
                ModelState.AddModelError<AccountView>(model => model.Email, Validations.EmailIsAlreadyUsed);

            return isUnique;
        }

        private Boolean IsAuthenticated(String username, String password)
        {
            String passhash = UnitOfWork
                .Repository<Account>()
                .Where(account => account.Username.ToLower() == username.ToLower())
                .Select(account => account.Passhash)
                .SingleOrDefault();

            Boolean isCorrect = passhash != null && hasher.Verify(password, passhash);
            if (!isCorrect)
                ModelState.AddModelError("", Validations.IncorrectUsernameOrPassword);

            return isCorrect;
        }
        private Boolean IsCorrectPassword(String accountId, String password)
        {
            String passhash = UnitOfWork
                .Repository<Account>()
                .Where(account => account.Id == accountId)
                .Select(account => account.Passhash)
                .Single();

            Boolean isCorrect = hasher.Verify(password, passhash);
            if (!isCorrect)
                ModelState.AddModelError<ProfileEditView>(model => model.Password, Validations.IncorrectPassword);

            return isCorrect;
        }

        private Boolean IsValidResetToken(String token)
        {
            Boolean isValid = UnitOfWork
                .Repository<Account>()
                .Any(account =>
                    account.RecoveryToken == token &&
                    account.RecoveryTokenExpirationDate > DateTime.Now);

            if (!isValid)
                Alerts.AddError(Validations.RecoveryTokenExpired);

            return isValid;
        }
    }
}
