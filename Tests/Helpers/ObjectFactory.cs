using NUnit.Framework;
using System;
using Template.Objects;
using Template.Tests.Objects;

namespace Template.Tests.Helpers
{
    public class ObjectFactory
    {
        public static String TestId
        {
            get
            {
                return TestContext.CurrentContext.Test.Name;
            }
        }

        public static Account CreateAccount(Int32 instanceNumber = 1)
        {
            return new Account()
            {
                Id = TestId + instanceNumber.ToString(),
                Username = "Username" + TestId + instanceNumber.ToString(),
                Passhash = "$2a$04$zNgYw403HgH1N69j4kj/peGI7SUvGiR5awIPZ2Yh/6O5BwyUO3qZe", // Password1
            };
        }
        public static LoginView CreateLoginView(Int32 instanceNumber = 1)
        {
            return new LoginView()
            {
                Id = TestId + instanceNumber.ToString(),
                Username = "Username" + TestId + instanceNumber.ToString(),
                Password = "Password1"
            };
        }
        public static ProfileView CreateProfileView(Int32 instanceNumber = 1)
        {
            return new ProfileView()
            {
                Username = "Username" + TestId + instanceNumber.ToString(),
                CurrentPassword = "Password1",
                NewPassword = "NewPassword1"
            };
        }
        public static AccountView CreateAccountView(Int32 instanceNumber = 1)
        {
            return new AccountView()
            {
                Id = TestId + instanceNumber.ToString(),
                Username = "Username" + TestId + instanceNumber.ToString(),
                RoleName = "RoleName" + TestId + instanceNumber.ToString(),
                RoleId = "RoleId" + TestId + instanceNumber.ToString(),
                Password = "Password1"
            };
        }

        public static Role CreateRole(Int32 instanceNumber = 1)
        {
            return new Role()
            {
                Id = TestId + instanceNumber.ToString(),
                Name = "Name" + TestId + instanceNumber.ToString()
            };
        }
        public static RoleView CreateRoleView(Int32 instanceNumber = 1)
        {
            return new RoleView()
            {
                Id = TestId + instanceNumber.ToString(),
                Name = "Name" + TestId + instanceNumber.ToString()
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
