using System;
using System.Linq;
using System.Text.RegularExpressions;
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
            isValid &= IsUniqueUsername(view);
            isValid &= IsPasswordLegal(view);

            return isValid;
        }
        public override Boolean CanEdit(AccountView view)
        {
            Boolean isValid = base.CanEdit(view);
            isValid &= IsUniqueUsername(view);

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
            account.Passhash = UnitOfWork.Repository<Account>().GetById(account.Id).Passhash;
            
            UnitOfWork.Repository<Account>().Update(account);
            UnitOfWork.Commit();
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
                ModelState.AddModelError("Username", Validations.UsernameIsAlreadyTaken);

            return isUnique;
        }
        private Boolean IsPasswordLegal(AccountView view)
        {
            Boolean isLegal = Regex.IsMatch(view.Password, "^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{8,}$");
            if (!isLegal)
                ModelState.AddModelError("Password", Validations.IllegalPassword);

            return isLegal;
        }
    }
}
