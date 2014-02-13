using System;

namespace Template.Components.Security
{
    public interface IRoleProvider
    {
        Boolean IsAuthorizedForAction();
        Boolean IsAuthorizedForAction(String action);
        Boolean IsAuthorizedForAction(String area, String controller, String action);
    }
}
