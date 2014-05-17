using System;
using System.Linq;
using System.Text.RegularExpressions;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.AkkountView;

namespace Template.Services
{
    public class AkkountsService : GenericService<Akkount, AkkountView>, IAkkountsService
    {
        public AkkountsService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override Boolean CanCreate(AkkountView view)
        {
            Boolean isValid = base.CanCreate(view);
            isValid &= IsUniqueUsername(view);
            isValid &= IsPasswordLegal(view);

            return isValid;
        }
        public override Boolean CanEdit(AkkountView view)
        {
            Boolean isValid = base.CanEdit(view);
            isValid &= IsUniqueUsername(view);

            return isValid;
        }

        public override void Create(AkkountView view)
        {
            Akkount akkount = UnitOfWork.ToModel<AkkountView, Akkount>(view);
            akkount.Passhash = BCrypter.HashPassword(view.Password);

            UnitOfWork.Repository<Akkount>().Insert(akkount);
            UnitOfWork.Commit();
        }
        public override void Edit(AkkountView view)
        {
            Akkount akkount = UnitOfWork.ToModel<AkkountView, Akkount>(view);
            akkount.Passhash = UnitOfWork.Repository<Akkount>().GetById(akkount.Id).Passhash;
            // TODO: Remove akkount username edit option
            UnitOfWork.Repository<Akkount>().Update(akkount);
            UnitOfWork.Commit();
        }

        private Boolean IsUniqueUsername(AkkountView view)
        {
            Boolean isUnique = !UnitOfWork
                .Repository<Akkount>()
                .Query(akkount =>
                    akkount.Id != view.Id &&
                    akkount.Username.ToUpper() == view.Username.ToUpper())
                .Any();

            if (!isUnique)
                ModelState.AddModelError("Username", Validations.UsernameIsAlreadyTaken);

            return isUnique;
        }
        private Boolean IsPasswordLegal(AkkountView view)
        {
            // TODO: Add null check on password
            Boolean isLegal = Regex.IsMatch(view.Password, "^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{8,}$");
            if (!isLegal)
                ModelState.AddModelError("Password", Validations.IllegalPassword);

            return isLegal;
        }
    }
}
