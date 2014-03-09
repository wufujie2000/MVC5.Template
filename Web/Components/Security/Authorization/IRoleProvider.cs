using System;

namespace Template.Components.Security
{
    public interface IRoleProvider
    {
        Boolean IsAuthorizedFor(String accountId, String area, String controller, String action);
    }
}
