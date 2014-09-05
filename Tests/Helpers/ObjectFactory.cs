using MvcTemplate.Objects;
using MvcTemplate.Tests.Objects;
using NUnit.Framework;
using System;

namespace MvcTemplate.Tests.Helpers
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
                Id = TestId + instanceNumber,
                Username = "Username" + TestId + instanceNumber,
                Passhash = "$2a$04$zNgYw403HgH1N69j4kj/peGI7SUvGiR5awIPZ2Yh/6O5BwyUO3qZe", // Password1
                Email = TestId + instanceNumber + "@tests.com",

                RecoveryToken = TestId + instanceNumber,
                RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(5)
            };
        }
        public static AccountView CreateAccountView(Int32 instanceNumber = 1)
        {
            return new AccountView()
            {
                Id = TestId + instanceNumber,
                Username = "Username" + TestId + instanceNumber,
                Email = TestId + instanceNumber + "@tests.com",
                Password = "Password1"
            };
        }
        public static ProfileEditView CreateProfileEditView(Int32 instanceNumber = 1)
        {
            return new ProfileEditView()
            {
                Id = TestId + instanceNumber,
                Username = "Username" + TestId + instanceNumber,
                Email = TestId + instanceNumber + "@tests.com",
                NewPassword = "NewPassword1",
                Password = "Password1"
            };
        }
        public static AccountEditView CreateAccountEditView(Int32 instanceNumber = 1)
        {
            return new AccountEditView()
            {
                Id = TestId + instanceNumber,
                Username = "Username" + TestId + instanceNumber,
                RoleName = "Name" + TestId + instanceNumber,
                RoleId = TestId + instanceNumber,
            };
        }
        public static AccountLoginView CreateAccountLoginView(Int32 instanceNumber = 1)
        {
            return new AccountLoginView()
            {
                Id = TestId + instanceNumber,
                Username = "Username" + TestId + instanceNumber,
                Password = "Password1"
            };
        }
        public static AccountRecoveryView CreateAccountRecoveryView(Int32 instanceNumber = 1)
        {
            return new AccountRecoveryView()
            {
                Id = TestId + instanceNumber,
                Email = TestId + instanceNumber + "@tests.com"
            };
        }
        public static AccountResetView CreateAccountResetView(Int32 instanceNumber = 1)
        {
            return new AccountResetView()
            {
                Id = TestId + instanceNumber,

                Token = TestId + instanceNumber,
                NewPassword = "NewPassword1"
            };
        }

        public static Role CreateRole(Int32 instanceNumber = 1)
        {
            return new Role()
            {
                Id = TestId + instanceNumber,
                Name = "Name" + TestId + instanceNumber
            };
        }
        public static RoleView CreateRoleView(Int32 instanceNumber = 1)
        {
            return new RoleView()
            {
                Id = TestId + instanceNumber,
                Name = "Name" + TestId + instanceNumber
            };
        }

        public static RolePrivilege CreateRolePrivilege(Int32 instanceNumber = 1)
        {
            return new RolePrivilege()
            {
                Id = TestId + instanceNumber
            };
        }
        public static RolePrivilegeView CreateRolePrivilegeView(Int32 instanceNumber = 1)
        {
            return new RolePrivilegeView()
            {
                Id = TestId + instanceNumber
            };
        }

        public static Privilege CreatePrivilege(Int32 instanceNumber = 1)
        {
            return new Privilege()
            {
                Id = TestId + instanceNumber,
                Area = "Area" + instanceNumber,
                Action = "Action" + instanceNumber,
                Controller = "Controller" + instanceNumber
            };
        }
        public static PrivilegeView CreatePrivilegeView(Int32 instanceNumber = 1)
        {
            return new PrivilegeView()
            {
                Id = TestId + instanceNumber,
                Controller = "Controller",
                Action = "Action",
                Area = "Area"
            };
        }

        public static TestModel CreateTestModel(Int32 instanceNumber = 1)
        {
            return new TestModel()
            {
                Id = TestId + instanceNumber,
                Text = "Text" + instanceNumber
            };
        }
    }
}
