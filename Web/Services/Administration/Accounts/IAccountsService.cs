using System;
using System.Collections.Generic;
using Template.Objects;

namespace Template.Services
{
    public interface IAccountsService : IService
    {
        Boolean CanCreate(AccountView view);
        Boolean CanEdit(AccountEditView view);

        IEnumerable<AccountView> GetViews();
        TView GetView<TView>(String id) where TView : BaseView;

        void Create(AccountView view);
        void Edit(AccountEditView view);
    }
}
