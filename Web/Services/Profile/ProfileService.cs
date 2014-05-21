using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Components.Extensions.Mvc;
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
            isValid &= IsLegalPassword(profile);

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
            UnitOfWork.Commit();

            AlertMessages.Add(AlertMessageType.Success, Messages.ProfileUpdated);
        }

        public void AddDeleteDisclaimerMessage()
        {
            AlertMessages.Add(AlertMessageType.Danger, Messages.ProfileDeleteDisclaimer, 0);
        }

        public virtual Boolean AccountExists(String accountId)
        {
            return UnitOfWork.Repository<Account>().Query(account => account.Id == accountId).Any();
        }

        private Boolean IsUniqueUsername(ProfileView profile)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id != HttpContext.Current.User.Identity.Name &&
                    account.Username.Trim().ToUpper() == profile.Username.Trim().ToUpper())
                 .Any();

            if (!isUnique)
                ModelState.AddModelError<ProfileView>(model => model.Username, Validations.UsernameIsAlreadyTaken);

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
                ModelState.AddModelError<ProfileView>(model => model.Username, Validations.IncorrectUsername);

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
                ModelState.AddModelError<ProfileView>(model => model.CurrentPassword, Validations.IncorrectPassword);

            return isCorrectPassword;
        }
        private Boolean IsLegalPassword(ProfileView profile)
        {
            if (String.IsNullOrWhiteSpace(profile.NewPassword)) return true;

            Boolean isLegal = Regex.IsMatch(profile.NewPassword, "^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{8,}$");
            if (!isLegal)
                ModelState.AddModelError<ProfileView>(model => model.NewPassword, Validations.IllegalPassword);

            return isLegal;
        }

        private Account GetAccountFrom(ProfileView profile)
        {
            Account account = UnitOfWork.Repository<Account>().GetById(HttpContext.Current.User.Identity.Name);
            account.Username = profile.Username.Trim();

            if (!String.IsNullOrWhiteSpace(profile.NewPassword))
                account.Passhash = BCrypter.HashPassword(profile.NewPassword);

            return account;
        }
    }
}
