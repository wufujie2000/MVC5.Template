using Template.Components.Security;
using Template.Objects;
using System;
using System.Linq;

namespace Template.Components.Services
{
    public class RoleProviderService : BaseService, IRoleProvider
    {
        private static RoleProviderService instance;
        public static RoleProviderService Instance
        {
            get
            {
                if (instance == null)
                    instance = new RoleProviderService();

                return instance;
            }
        }

        private RoleProviderService()
        {
        }

        public virtual Boolean IsAuthorizedForAction()
        {
            return IsAuthorizedForAction(CurrentArea, CurrentController, CurrentAction);
        }
        public virtual Boolean IsAuthorizedForAction(String action)
        {
            return IsAuthorizedForAction(CurrentArea, CurrentController, action);
        }
        public virtual Boolean IsAuthorizedForAction(String area, String controller, String action)
        {
            Boolean isAuthorized = UnitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id == CurrentAccountId &&
                    account.User.Role.RolePrivileges.Any(rolePrivilege =>
                        rolePrivilege.Privilege.Area == area &&
                        rolePrivilege.Privilege.Controller == controller &&
                        rolePrivilege.Privilege.Action == action))
                .Any();

            return isAuthorized;   
        }
    }
}
