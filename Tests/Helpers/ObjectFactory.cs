using NUnit.Framework;
using System;
using Template.Objects;
using Template.Tests.Objects.Components.Services;

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
        public static ProfileView CreateProfileView(Int32 instanceNumber = 1)
        {
            return new ProfileView()
            {
                Username = "Username" + TestId + instanceNumber.ToString(),
                CurrentPassword = "Password1",
                NewPassword = "NewPassword1",
                Person = new PersonView()
                {
                    LastName = "LastName" + instanceNumber.ToString(),
                    FirstName = "FirstName" + instanceNumber.ToString(),
                    DateOfBirth = new DateTime(1990, 1, 1).AddDays(instanceNumber)
                }
            };
        }
        public static AccountView CreateAccountView(Int32 instanceNumber = 1)
        {
            return new AccountView()
            {
                Id = TestId + instanceNumber.ToString(),
                Username = "Username" + TestId + instanceNumber.ToString(),
                Password = "Password1"
            };
        }
        public static UserView CreateUserView(Int32 instanceNumber = 1)
        {
            return new UserView()
            {
                Id = TestId + instanceNumber.ToString(),
                Person = new PersonView()
                {
                    LastName = "LastName" + instanceNumber.ToString(),
                    FirstName = "FirstName" + instanceNumber.ToString(),
                    DateOfBirth = new DateTime(1990, 1, 1).AddDays(instanceNumber)
                },

                Username = "Username" + TestId + instanceNumber.ToString(),
                Password = "Password1"
            };
        }

        public static Person CreatePerson(Int32 instanceNumber = 1)
        {
            return new Person()
            {
                Id = TestId + instanceNumber.ToString(),
                LastName = "LastName" + instanceNumber.ToString(),
                FirstName = "FirstName" + instanceNumber.ToString(),
                DateOfBirth = new DateTime(1990, 1, 1).AddDays(instanceNumber)
            };
        }
        public static PersonView CreatePersonView(Int32 instanceNumber = 1)
        {
            return new PersonView()
            {
                Id = TestId + instanceNumber.ToString(),
                LastName = "LastName" + instanceNumber.ToString(),
                FirstName = "FirstName" + instanceNumber.ToString(),
                DateOfBirth = new DateTime(1990, 1, 1).AddDays(instanceNumber),

                RoleId = TestId + instanceNumber.ToString()
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
