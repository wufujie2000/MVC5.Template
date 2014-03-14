using System;

namespace Template.Objects
{
    public class Person : BaseModel
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public String RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
