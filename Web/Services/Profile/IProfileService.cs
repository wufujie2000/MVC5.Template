using System;
using System.Collections.Generic;
using Template.Objects;

namespace Template.Services
{
    public interface IProfileService : IService
    {
        Boolean AccountExists(String accountId);

        Boolean CanEdit(ProfileEditView profile);
        Boolean CanDelete(AccountView profile);

        TView GetView<TView>(String id) where TView : BaseView;

        void Edit(ProfileEditView profile);
        void Delete(String id);
    }
}
