using System;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Components.Security;
using Template.Objects;
using Template.Resources.Views.AccountView;
using Template.Resources.Views.ProfileView;

namespace Template.Components.Services
{
    public class ProfileService : GenericService<User, ProfileView>
    {
        public ProfileService(ModelStateDictionary modelState)
            : base(modelState)
        {
        }

        public ProfileView GetView()
        {
            return UnitOfWork
                .Repository<Account>()
                .ProjectTo<ProfileView>(account => account.Id == CurrentAccountId)
                .First();
        }

        public override Boolean CanEdit(ProfileView profile)
        {
            Boolean isValid = base.CanEdit(profile);
            isValid &= IsUniqueUsername(profile);
            isValid &= IsCorrectPassword(profile);

            return isValid;
        }
        private Boolean IsUniqueUsername(ProfileView profile)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id != CurrentAccountId &&
                    account.Username == profile.Username)
                 .Any();

            if (!isUnique)
                ModelState.AddModelError("Username", Validations.UsernameIsAlreadyTaken);

            return isUnique;
        }
        private Boolean IsCorrectPassword(ProfileView profile)
        {
            String currentPasshash = UnitOfWork
                .Repository<Account>()
                .Query(account => account.Id == CurrentAccountId)
                .Select(account => account.Passhash)
                .First();

            Boolean isCorrectPassword = BCrypter.Verify(profile.CurrentPassword, currentPasshash);
            if (!isCorrectPassword)
                ModelState.AddModelError("CurrentPassword", Validations.IncorrectPassword);

            return isCorrectPassword;
        }

        public override void Edit(ProfileView profile)
        {
            UnitOfWork.Repository<Account>().Update(GetAccountFrom(profile));
            UnitOfWork.Repository<User>().Update(GetUserFrom(profile));
            UnitOfWork.Commit();

            AlertMessages.Add(AlertMessageType.Success, Messages.ProfileUpdated);
        }

        private Account GetAccountFrom(ProfileView profile)
        {
            // TODO: Create mapping from ProfileView to Account and User models?
            // TODO: Profile view still does not have delete and details links.
            var account = UnitOfWork.Repository<Account>().GetById(CurrentAccountId);
            account.Username = profile.Username.ToLower();
            if (profile.NewPassword != null)
                account.Passhash = BCrypter.HashPassword(profile.NewPassword);

            return account;
        }
        private User GetUserFrom(ProfileView profile)
        {
            var user = UnitOfWork.Repository<Account>().GetById(CurrentAccountId).User;
            user.FirstName = profile.UserFirstName;
            user.LastName = profile.UserLastName;
            user.DateOfBirth = profile.UserDateOfBirth;

            return user;
        }
    }
}
