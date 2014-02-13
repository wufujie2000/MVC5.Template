using Template.Components.Security;
using Template.Objects;
using Template.Resources.Views.AccountView;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Template.Components.Services
{
    public class UsersService : GenericService<Account, UserView>
    {
        public UsersService(ModelStateDictionary modelState)
            : base(modelState)
        {
        }

        public override Boolean CanCreate(UserView view)
        {
            Boolean isValid = base.CanCreate(view);
            isValid &= IsUniqueUsername(view);
            isValid &= IsPasswordSpecified(view);

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
            view.Username = view.Username.ToLower();
            var user = UnitOfWork.ToModel<UserView, User>(view);
            var account = UnitOfWork.ToModel<UserView, Account>(view);

            account.Passhash = BCrypter.HashPassword(view.Password);
            account.UserId = user.Id;

            UnitOfWork.Repository<User>().Insert(user);
            UnitOfWork.Repository<Account>().Insert(account);        
            UnitOfWork.Commit();
        }
        public override void Edit(UserView view)
        {
            view.Username = view.Username.ToLower();
            var user = UnitOfWork.ToModel<UserView, User>(view);
            var account = UnitOfWork.ToModel<UserView, Account>(view);
            if (view.NewPassword == null)
                account.Passhash = UnitOfWork.Repository<Account>().GetById(account.Id).Passhash;
            else
                account.Passhash = BCrypter.HashPassword(view.NewPassword);

            UnitOfWork.Repository<Account>().Update(account);
            UnitOfWork.Repository<User>().Update(user);
            UnitOfWork.Commit();
        }
        public override void Delete(String id)
        {
            UnitOfWork.Repository<User>().Delete(id);
            UnitOfWork.Commit();
        }

        private Boolean IsUniqueUsername(UserView user)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id != user.Id &&
                    account.Username.ToLower() == user.Username.ToLower())
                 .Any();

            if (!isUnique)
                ModelState.AddModelError("Username", Validations.UsernameIsAlreadyTaken);
            
            return isUnique;
        }
        private Boolean IsPasswordSpecified(UserView user)
        {
            Boolean isSpecified = !String.IsNullOrWhiteSpace(user.Password);
            if (!isSpecified)
                ModelState.AddModelError("Password", Validations.PasswordFieldIsRequired);

            return isSpecified;
        }
    }
}
