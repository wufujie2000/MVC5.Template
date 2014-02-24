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
    public class ProfileService : GenericService<Account, ProfileView>
    {
        public ProfileService(ModelStateDictionary modelState)
            : base(modelState)
        {
        }

        public override Boolean CanEdit(ProfileView profile)
        {
            Boolean isValid = base.CanEdit(profile);
            isValid &= IsUniqueUsername(profile);
            isValid &= IsCorrectPassword(profile);

            return isValid;
        }
        public Boolean CanDelete(ProfileView profile)
        {
            Boolean isValid = CanDelete(profile.Id);
            isValid &= IsCorrectUsername(profile);
            isValid &= IsCorrectPassword(profile);

            return isValid;
        }

        public override void Edit(ProfileView profile)
        {
            UnitOfWork.Repository<Account>().Update(GetAccountFrom(profile));
            UnitOfWork.Repository<User>().Update(GetUserFrom(profile));
            UnitOfWork.Commit();

            AlertMessages.Add(AlertMessageType.Success, Messages.ProfileUpdated);
        }

        private Boolean IsUniqueUsername(ProfileView profile)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id != CurrentAccountId &&
                    account.Username == profile.Username)
                 .Any();
            // TODO: Change validation messages to ProfileView not AccountView
            // TODO: Change ToLower to ToUpperInvariant
            if (!isUnique)
                ModelState.AddModelError("Username", Validations.UsernameIsAlreadyTaken);

            return isUnique;
        }
        private Boolean IsCorrectUsername(ProfileView profile)
        {
            String profileUsername = UnitOfWork
                .Repository<Account>()
                .Query(account => account.Id == CurrentAccountId)
                .Select(account => account.Username.ToLower())
                .First();

            Boolean isCorrectUsername = profileUsername == profile.Username.ToLower();
            if (!isCorrectUsername)
                ModelState.AddModelError("Username", Validations.IncorrectUsername);

            return isCorrectUsername;
        }
        private Boolean IsCorrectPassword(ProfileView profile)
        {
            String profilePasshash = UnitOfWork
                .Repository<Account>()
                .Query(account => account.Id == CurrentAccountId)
                .Select(account => account.Passhash)
                .First();

            Boolean isCorrectPassword = BCrypter.Verify(profile.CurrentPassword, profilePasshash);
            if (!isCorrectPassword)
                ModelState.AddModelError("CurrentPassword", Validations.IncorrectPassword);

            return isCorrectPassword;
        }

        private Account GetAccountFrom(ProfileView profile)
        {
            // TODO: Create mapping from ProfileView to Account and User models?
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
