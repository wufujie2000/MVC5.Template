using System;

namespace MvcTemplate.Components.Security
{
    public interface IAuthProvider
    {
        Boolean IsAuthorizedFor(String accountId, String area, String controller, String action);

        void Refresh();
    }
}
