using System;
using System.Collections.Generic;

namespace Template.Components.Security
{
    public interface IRoleProvider
    {
        IEnumerable<AccountPrivilege> GetAccountPrivileges(String accountId);

        Boolean IsAuthorizedFor(String accountId, String area, String controller, String action);
        Boolean IsAuthorizedFor(IEnumerable<AccountPrivilege> privileges, String area, String controller, String action);
    }
}
