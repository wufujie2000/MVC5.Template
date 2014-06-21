using System;
using System.Linq;
using System.Text.RegularExpressions;
using Template.Components.Extensions.Mvc;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.AccountView;

namespace Template.Services
{
    public class AccountsService : GenericService<Account, AccountView>, IAccountsService
    {
        public AccountsService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override Boolean CanCreate(AccountView view)
        {
            Boolean isValid = base.CanCreate(view);
            isValid &= IsUsernameSpecified(view);
            isValid &= IsUniqueUsername(view);
            isValid &= IsLegalPassword(view);

            isValid &= IsEmailSpecified(view);
            isValid &= IsUniqueEmail(view);

            return isValid;
        }

        public override void Create(AccountView view)
        {
            Account account = UnitOfWork.ToModel<AccountView, Account>(view);
            account.Passhash = BCrypter.HashPassword(view.Password);

            UnitOfWork.Repository<Account>().Insert(account);
            UnitOfWork.Commit();
        }
        public override void Edit(AccountView view)
        {
            Account account = UnitOfWork.ToModel<AccountView, Account>(view);
            Account accountInDatabase = UnitOfWork.Repository<Account>().GetById(account.Id);

            account.Username = accountInDatabase.Username;
            account.Passhash = accountInDatabase.Passhash;
            account.Email = accountInDatabase.Email;

            UnitOfWork.Repository<Account>().Update(account);
            UnitOfWork.Commit();
        }

        private Boolean IsUsernameSpecified(AccountView view)
        {
            Boolean isSpecified = !String.IsNullOrEmpty(view.Username);

            if (!isSpecified)
            {
                String errorMessage = String.Format(Resources.Shared.Validations.FieldIsRequired, Titles.Username);
                ModelState.AddModelError<AccountView>(model => model.Username, errorMessage);
            }

            return isSpecified;
        }
        private Boolean IsUniqueUsername(AccountView view)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id != view.Id &&
                    account.Username.ToUpper() == view.Username.ToUpper())
                .Any();

            if (!isUnique)
                ModelState.AddModelError<AccountView>(model => model.Username, Validations.UsernameIsAlreadyTaken);

            return isUnique;
        }
        private Boolean IsLegalPassword(AccountView view)
        {
            Boolean isLegal = Regex.IsMatch(view.Password ?? String.Empty, "^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{8,}$");
            if (!isLegal)
                ModelState.AddModelError<AccountView>(model => model.Password, Validations.IllegalPassword);

            return isLegal;
        }
        private Boolean IsEmailSpecified(AccountView view)
        {
            Boolean isSpecified = !String.IsNullOrEmpty(view.Email);

            if (!isSpecified)
            {
                String errorMessage = String.Format(Resources.Shared.Validations.FieldIsRequired, Titles.Email);
                ModelState.AddModelError<AccountView>(model => model.Email, errorMessage);
            }

            return isSpecified;
        }
        private Boolean IsUniqueEmail(AccountView view)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account => account.Email.ToUpper() == view.Email.ToUpper())
                .Any();

            if (!isUnique)
                ModelState.AddModelError<AccountView>(model => model.Email, Validations.EmailIsAlreadyUsed);

            return isUnique;
        }
    }
}
