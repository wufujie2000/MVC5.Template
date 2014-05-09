using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.UserView;

namespace Template.Services
{
    public class UsersService : GenericService<Account, UserView>, IUsersService
    {
        public UsersService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override Boolean CanCreate(UserView view)
        {
            Boolean isValid = base.CanCreate(view);
            isValid &= IsUniqueUsername(view);
            isValid &= IsPasswordLegal(view);

            return isValid;
        }
        public override Boolean CanEdit(UserView view)
        {
            Boolean isValid = base.CanEdit(view);
            isValid &= IsUniqueUsername(view);

            return isValid;
        }
        
        public override void Create(UserView view)
        {
            Account account = UnitOfWork.ToModel<UserView, Account>(view);
            account.Passhash = BCrypter.HashPassword(view.Password);

            UnitOfWork.Repository<Account>().Insert(account);        
            UnitOfWork.Commit();
        }
        public override void Edit(UserView view)
        {
            Account account = UnitOfWork.ToModel<UserView, Account>(view);
            account.Passhash = UnitOfWork.Repository<Account>().GetById(account.Id).Passhash;
            Person person = account.Person;

            UnitOfWork.Repository<Account>().Update(account);
            UnitOfWork.Repository<Person>().Update(person);
            UnitOfWork.Commit();
        }
        public override void Delete(String id)
        {
            UnitOfWork.Repository<Person>().Delete(id);
            UnitOfWork.Commit();
        }

        private Boolean IsUniqueUsername(UserView view)
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
        private Boolean IsPasswordLegal(UserView view)
        {
            Boolean isLegal = Regex.IsMatch(view.Password, "^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{8,}$");
            if (!isLegal)
                ModelState.AddModelError("Password", Validations.IllegalPassword);

            return isLegal;
        }
    }
}
