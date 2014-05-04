using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.ProfileView;

namespace Template.Services
{
    public class ProfileService : GenericService<Account, ProfileView>, IProfileService
    {
        public ProfileService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override Boolean CanEdit(ProfileView profile)
        {
            Boolean isValid = base.CanEdit(profile);
            isValid &= IsCorrectPassword(profile);
            isValid &= IsUniqueUsername(profile);
            isValid &= IsPasswordLegal(profile);

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
            Account account = GetAccountFrom(profile);
            UnitOfWork.Repository<Account>().Update(account);
            UnitOfWork.Repository<Person>().Update(account.Person);
            UnitOfWork.Commit();

            AlertMessages.Add(AlertMessageType.Success, Messages.ProfileUpdated);
        }

        public override void Delete(String id)
        {
            UnitOfWork.Repository<Person>().Delete(id);
            UnitOfWork.Commit();
        }

        public void AddDeleteDisclaimerMessage()
        {
            AlertMessages.Add(AlertMessageType.Danger, Messages.ProfileDeleteDisclaimer, 0);
        }

        private Boolean IsUniqueUsername(ProfileView profile)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id != HttpContext.Current.User.Identity.Name &&
                    account.Username == profile.Username)
                 .Any();

            if (!isUnique)
                ModelState.AddModelError("Username", Validations.UsernameIsAlreadyTaken);

            return isUnique;
        }
        private Boolean IsCorrectUsername(ProfileView profile)
        {
            String username = UnitOfWork
                .Repository<Account>()
                .Query(account => account.Id == HttpContext.Current.User.Identity.Name)
                .Select(account => account.Username)
                .First();
            
            Boolean isCorrectUsername = username.ToUpperInvariant() == profile.Username.ToUpperInvariant();
            if (!isCorrectUsername)
                ModelState.AddModelError("Username", Validations.IncorrectUsername);

            return isCorrectUsername;
        }
        private Boolean IsCorrectPassword(ProfileView profile)
        {
            String profilePasshash = UnitOfWork
                .Repository<Account>()
                .Query(account => account.Id == HttpContext.Current.User.Identity.Name)
                .Select(account => account.Passhash)
                .First();

            Boolean isCorrectPassword = BCrypter.Verify(profile.CurrentPassword, profilePasshash);
            if (!isCorrectPassword)
                ModelState.AddModelError("CurrentPassword", Validations.IncorrectPassword);

            return isCorrectPassword;
        }
        private Boolean IsPasswordLegal(ProfileView profile)
        {
            if (profile.NewPassword == null) return true;

            Boolean isLegal = Regex.IsMatch(profile.NewPassword, "^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{8,}$");
            if (!isLegal)
                ModelState.AddModelError("NewPassword", Validations.IllegalPassword);

            return isLegal;
        }

        private Account GetAccountFrom(ProfileView profile)
        {
            Account account = UnitOfWork.Repository<Account>().GetById(HttpContext.Current.User.Identity.Name);
            account.Username = profile.Username;
            if (profile.NewPassword != null)
                account.Passhash = BCrypter.HashPassword(profile.NewPassword);

            account.Person.DateOfBirth = profile.Person.DateOfBirth;
            account.Person.FirstName = profile.Person.FirstName;
            account.Person.LastName = profile.Person.LastName;

            return account;
        }
    }
}
