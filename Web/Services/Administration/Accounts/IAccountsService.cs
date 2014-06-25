using System;
using Template.Objects;

namespace Template.Services
{
    public interface IAccountsService : IGenericService<AccountView>
    {
        Boolean CanCreate(AccountView view);
        Boolean CanEdit(AccountView view);

        void Create(AccountView view);
        void Edit(AccountView view);
    }
}
