using NUnit.Framework;
using System;
using Template.Objects;

namespace Template.Tests.Helpers
{
    public class ObjectFactory
    {
        private static String TestId
        {
            get
            {
                return TestContext.CurrentContext.Test.Name;
            }
        }

        public static ProfileView CreateProfileView()
        {
            return new ProfileView()
            {
                Id = TestId,
                Username = "Username",
                CurrentPassword = "Password",
                NewPassword = "NewPassword",
                UserFirstName = "FirstName",
                UserLastName = "LastName",
                UserDateOfBirth = new DateTime(1990, 1, 1)
            };
        }

        public static Account CreateAccount()
        {
            return new Account()
            {
                Id = TestId,
                Username = "Username",
                Passhash = "$2a$04$aalrk68zd5esaX9/ihL//OwwN/ORg12CToxkgXCzK0jfH0z1h/PK.", // Password
            };
        }
        public static AccountView CreateAccountView()
        {
            return new AccountView()
            {
                Id = TestId,
                Username = "Username",
                Password = "Password",
                NewPassword = "NewPassword"
            };
        }

        public static User CreateUser()
        {
            return new User()
            {
                Id = TestId,
                FirstName = "FirstName",
                LastName = "LastName",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
        }
        public static UserView CreateUserView()
        {
            return new UserView()
            {
                Id = TestId,
                UserFirstName = "FirstName",
                UserLastName = "LastName",
                UserDateOfBirth = new DateTime(1990, 1, 1),

                Username = "Username",
                Password = "Password",
                NewPassword = "NewPassword"
            };
        }

        public static Role CreateRole()
        {
            return new Role()
            {
                Id = TestId,
                Name = "Name"
            };
        }
        public static RoleView CreateRoleView()
        {
            return new RoleView()
            {
                Id = TestId,
                Name = "Name"
            };
        }

        public static RolePrivilege CreateRolePrivilege(Int32 instanceIndex = 1)
        {
            return new RolePrivilege()
            {
                Id = TestId + instanceIndex.ToString()
            };
        }
        public static RolePrivilegeView CreateRolePrivilegeView()
        {
            return new RolePrivilegeView()
            {
                Id = TestId
            };
        }

        public static Privilege CreatePrivilege(Int32 instanceNumber = 1)
        {
            return new Privilege()
            {
                Id = TestId + instanceNumber.ToString(),
                Area = "Area",
                Controller = "Controller",
                Action = "Action"
            };
        }
        public static PrivilegeView CreatePrivilegeView()
        {
            return new PrivilegeView()
            {
                Id = TestId,
                Area = "Area",
                Controller = "Controller",
                Action = "Action"
            };
        }

        public static PrivilegeLanguage CreatePrivilegeLanguage(Int32 instanceIndex = 1)
        {
            return new PrivilegeLanguage()
            {
                Id = TestId + instanceIndex.ToString(),
                Area = "LanguageArea" + instanceIndex.ToString(),
                Controller = "LanguageController" + instanceIndex.ToString(),
                Action = "LanguageAction" + instanceIndex.ToString()
            };
        }

        public static Language CreateLanguage()
        {
            return new Language()
            {
                Id = TestId,
                Name = "Name",
                Abbreviation = "Abbreviation"
            };
        }
    }
}
