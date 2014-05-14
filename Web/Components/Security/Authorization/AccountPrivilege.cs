using System;

namespace Template.Components.Security
{
    public class AccountPrivilege
    {
        public String AccountId { get; set; }

        public String Area { get; set; }
        public String Controller { get; set; }
        public String Action { get; set; }
    }
}
