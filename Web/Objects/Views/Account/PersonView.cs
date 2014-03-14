using System;

namespace Template.Objects
{
    public class PersonView : BaseView
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public String RoleId { get; set; }

        public virtual RoleView Role { get; set; }
    }
}
