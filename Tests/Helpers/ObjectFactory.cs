using NUnit.Framework;
using System;
using Template.Objects;
using Template.Tests.Objects.Components.Services;

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
                Username = "Username" + TestId,
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
                Username = "Username" + TestId,
                Passhash = "$2a$04$aalrk68zd5esaX9/ihL//OwwN/ORg12CToxkgXCzK0jfH0z1h/PK.", // Password
            };
        }
        public static AccountView CreateAccountView()
        {
            return new AccountView()
            {
                Id = TestId,
                Username = "Username" + TestId,
                Password = "Password",
                NewPassword = "NewPassword"
            };
        }

        public static User CreateUser(Int32 instanceNumber = 1)
        {
            return new User()
            {
                Id = TestId + instanceNumber.ToString(),
                FirstName = "FirstName" + instanceNumber.ToString(),
                LastName = "LastName" + instanceNumber.ToString(),
                DateOfBirth = new DateTime(1990, 1, 1).AddDays(instanceNumber)
            };
        }
        public static UserView CreateUserView(Int32 instanceNumber = 1)
        {
            return new UserView()
            {
                Id = TestId + instanceNumber.ToString(),
                UserFirstName = "FirstName" + instanceNumber.ToString(),
                UserLastName = "LastName" + instanceNumber.ToString(),
                UserDateOfBirth = new DateTime(1990, 1, 1).AddDays(instanceNumber),

                UserRoleId = TestId + instanceNumber.ToString(),
                UserRoleName = "UserRoleName" + instanceNumber.ToString(),

                Username = "Username" + TestId,
                Password = "Password",
                NewPassword = "NewPassword"
            };
        }

        public static Role CreateRole(Int32 instanceNumber = 1)
        {
            return new Role()
            {
                Id = TestId + instanceNumber.ToString(),
                Name = "Name" + instanceNumber.ToString()
            };
        }
        public static RoleView CreateRoleView(Int32 instanceNumber = 1)
        {
            return new RoleView()
            {
                Id = TestId + instanceNumber.ToString(),
                Name = "Name" + instanceNumber.ToString()
            };
        }

        public static RolePrivilege CreateRolePrivilege(Int32 instanceNumber = 1)
        {
            return new RolePrivilege()
            {
                Id = TestId + instanceNumber.ToString()
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
                Area = "Area" + instanceNumber.ToString(),
                Controller = "Controller" + instanceNumber.ToString(),
                Action = "Action" + instanceNumber.ToString()
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

        public static Language CreateLanguage(Int32 instanceNumber = 1)
        {
            return new Language()
            {
                Id = TestId + instanceNumber.ToString(),
                Name = "Name" + instanceNumber.ToString(),
                Abbreviation = "Abbreviation" + instanceNumber.ToString()
            };
        }
        public static LanguageView CreateLanguageView(Int32 instanceNumber = 1)
        {
            return new LanguageView()
            {
                Id = TestId + instanceNumber.ToString(),
                Name = "Name" + instanceNumber.ToString(),
                Abbreviation = "Abbreviation" + instanceNumber.ToString()
            };
        }

        public static TestModel CreateTestModel(Int32 instanceNumber = 1)
        {
            return new TestModel()
            {
                Id = TestId + instanceNumber.ToString(),
                Text = "Text" + instanceNumber.ToString()
            };
        }
        public static TestView CreateTestView(Int32 instanceNumber = 1)
        {
            return new TestView()
            {
                Id = TestId + instanceNumber.ToString(),
                Text = "Text" + instanceNumber.ToString()
            };
        }
    }
}
