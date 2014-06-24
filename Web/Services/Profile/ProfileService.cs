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
        private IHasher hasher;

        public ProfileService(IUnitOfWork unitOfWork, IHasher hasher)
            : base(unitOfWork)
        {
            this.hasher = hasher;
        }

        public virtual Boolean AccountExists(String accountId)
        {
            return UnitOfWork.Repository<Account>().Query(account => account.Id == accountId).Any();
        }
        public override Boolean CanEdit(ProfileView profile)
        {
            Boolean isValid = base.CanEdit(profile);
            isValid &= IsCorrectPassword(profile);
            isValid &= IsUniqueUsername(profile);
            isValid &= IsLegalPassword(profile);

            isValid &= IsEmailSpecified(profile);
            isValid &= IsUniqueEmail(profile);

            return isValid;
        }
        public Boolean CanDelete(ProfileView profile)
        {
            return IsCorrectPassword(profile);
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

        private Boolean IsCorrectPassword(ProfileView profile)
        {
            String profilePasshash = UnitOfWork
                .Repository<Account>()
                .Query(account => account.Id == HttpContext.Current.User.Identity.Name)
                .Select(account => account.Passhash)
                .First();

            Boolean isCorrectPassword = hasher.Verify(profile.CurrentPassword, profilePasshash);
            if (!isCorrectPassword)
                ModelState.AddModelError<ProfileView>(model => model.CurrentPassword, Validations.IncorrectPassword);

            return isCorrectPassword;
        }
        private Boolean IsUniqueUsername(ProfileView profile)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id != HttpContext.Current.User.Identity.Name &&
                    account.Username.ToUpper() == profile.Username.ToUpper())
                 .Any();

            if (!isUnique)
                ModelState.AddModelError<ProfileView>(model => model.Username, Validations.UsernameIsAlreadyTaken);

            return isUnique;
        }
        private Boolean IsLegalPassword(ProfileView profile)
        {
            if (String.IsNullOrWhiteSpace(profile.NewPassword)) return true;

            Boolean isLegal = Regex.IsMatch(profile.NewPassword, "^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{8,}$");
            if (!isLegal)
                ModelState.AddModelError<ProfileView>(model => model.NewPassword, Validations.IllegalPassword);

            return isLegal;
        }
        private Boolean IsEmailSpecified(ProfileView profile)
        {
            Boolean isSpecified = !String.IsNullOrEmpty(profile.Email);

            if (!isSpecified)
            {
                String errorMessage = String.Format(Resources.Shared.Validations.FieldIsRequired, Titles.Email);
                ModelState.AddModelError<ProfileView>(model => model.Email, errorMessage);
            }

            return isSpecified;
        }
        private Boolean IsUniqueEmail(ProfileView profile)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id != HttpContext.Current.User.Identity.Name &&
                    account.Email.ToUpper() == profile.Email.ToUpper())
                .Any();

            if (!isUnique)
                ModelState.AddModelError<ProfileView>(model => model.Email, Validations.EmailIsAlreadyUsed);

            return isUnique;
        }

        private Account GetAccountFrom(ProfileView profile)
        {
            Account account = UnitOfWork.Repository<Account>().GetById(HttpContext.Current.User.Identity.Name);
            account.Username = profile.Username;

            if (!String.IsNullOrWhiteSpace(profile.NewPassword))
                account.Passhash = hasher.HashPassword(profile.NewPassword);

            return account;
        }
    }
}
