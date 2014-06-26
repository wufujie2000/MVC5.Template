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
using Template.Resources.Views.AccountView;

namespace Template.Services
{
    public class ProfileService : BaseService, IProfileService
    {
        private IHasher hasher;

        public ProfileService(IUnitOfWork unitOfWork, IHasher hasher)
            : base(unitOfWork)
        {
            this.hasher = hasher;
        }

        public Boolean AccountExists(String accountId)
        {
            return UnitOfWork.Repository<Account>().Query(account => account.Id == accountId).Any();
        }

        public Boolean CanEdit(ProfileEditView profile)
        {
            Boolean isValid = ModelState.IsValid;
            isValid &= IsCorrectPassword(profile);
            isValid &= IsUniqueUsername(profile);
            isValid &= IsLegalPassword(profile);

            isValid &= IsEmailSpecified(profile);
            isValid &= IsUniqueEmail(profile);

            return isValid;
        }
        public Boolean CanDelete(AccountView profile)
        {
            ProfileEditView view = new ProfileEditView();
            view.Password = profile.Password;

            return IsCorrectPassword(view);
        }

        public TView GetView<TView>(String id) where TView : BaseView
        {
            return UnitOfWork.Repository<Account>().GetById<TView>(id);
        }

        public void Edit(ProfileEditView profile)
        {
            Account account = GetAccountFrom(profile);
            UnitOfWork.Repository<Account>().Update(account);
            UnitOfWork.Commit();

            AlertMessages.Add(AlertMessageType.Success, Messages.ProfileUpdated);
        }
        public void Delete(String id)
        {
            UnitOfWork.Repository<Account>().Delete(id);
            UnitOfWork.Commit();
        }

        private Boolean IsCorrectPassword(ProfileEditView profile)
        {
            String profilePasshash = UnitOfWork
                .Repository<Account>()
                .Query(account => account.Id == HttpContext.Current.User.Identity.Name)
                .Select(account => account.Passhash)
                .First();

            Boolean isCorrectPassword = hasher.Verify(profile.Password, profilePasshash);
            if (!isCorrectPassword)
                ModelState.AddModelError<ProfileEditView>(model => model.Password, Validations.IncorrectPassword);

            return isCorrectPassword;
        }
        private Boolean IsUniqueUsername(ProfileEditView profile)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id != HttpContext.Current.User.Identity.Name &&
                    account.Username.ToUpper() == profile.Username.ToUpper())
                 .Any();

            if (!isUnique)
                ModelState.AddModelError<ProfileEditView>(model => model.Username, Validations.UsernameIsAlreadyTaken);

            return isUnique;
        }
        private Boolean IsLegalPassword(ProfileEditView profile)
        {
            if (String.IsNullOrWhiteSpace(profile.NewPassword)) return true;

            Boolean isLegal = Regex.IsMatch(profile.NewPassword, "^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{8,}$");
            if (!isLegal)
                ModelState.AddModelError<ProfileEditView>(model => model.NewPassword, Validations.IllegalPassword);

            return isLegal;
        }
        private Boolean IsEmailSpecified(ProfileEditView profile)
        {
            Boolean isSpecified = !String.IsNullOrEmpty(profile.Email);

            if (!isSpecified)
            {
                String errorMessage = String.Format(Resources.Shared.Validations.FieldIsRequired, Titles.Email);
                ModelState.AddModelError<ProfileEditView>(model => model.Email, errorMessage);
            }

            return isSpecified;
        }
        private Boolean IsUniqueEmail(ProfileEditView profile)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id != HttpContext.Current.User.Identity.Name &&
                    account.Email.ToUpper() == profile.Email.ToUpper())
                .Any();

            if (!isUnique)
                ModelState.AddModelError<ProfileEditView>(model => model.Email, Validations.EmailIsAlreadyUsed);

            return isUnique;
        }

        private Account GetAccountFrom(ProfileEditView profile)
        {
            Account account = UnitOfWork.Repository<Account>().GetById(HttpContext.Current.User.Identity.Name);
            account.Username = profile.Username;

            if (!String.IsNullOrWhiteSpace(profile.NewPassword))
                account.Passhash = hasher.HashPassword(profile.NewPassword);

            return account;
        }
    }
}
