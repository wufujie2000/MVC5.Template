using MvcTemplate.Objects;
using System;

namespace MvcTemplate.Validators
{
    public interface IAccountValidator : IValidator
    {
        Boolean CanLogin(AccountLoginView view);
        Boolean CanRegister(AccountView view);
        Boolean CanEdit(ProfileEditView view);
        Boolean CanEdit(AccountEditView view);
        Boolean CanDelete(AccountView view);
    }
}
