using MvcTemplate.Objects;
using MvcTemplate.Tests.Objects;
using System;

namespace MvcTemplate.Tests
{
    public class ObjectFactory
    {
        public static Account CreateAccount(String id = "1")
        {
            return new Account
            {
                Id = id,
                Username = "Username" + id,
                Passhash = "$2a$04$zNgYw403HgH1N69j4kj/peGI7SUvGiR5awIPZ2Yh/6O5BwyUO3qZe", // Password1
                Email = id + "@tests.com",

                RecoveryToken = "Token" + id,
                RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(5)
            };
        }
        public static AccountView CreateAccountView(String id = "1")
        {
            return new AccountView
            {
                Id = id,
                Username = "Username" + id,
                Email = id + "@tests.com",
                Password = "Password1"
            };
        }

        public static ProfileEditView CreateProfileEditView(String id = "1")
        {
            return new ProfileEditView
            {
                Id = id,
                Username = "Username" + id,
                Email = id + "@tests.com",
                NewPassword = "NewPassword1",
                Password = "Password1"
            };
        }
        public static ProfileDeleteView CreateProfileDeleteView(String id = "1")
        {
            return new ProfileDeleteView
            {
                Id = id,
                Password = "Password1"
            };
        }

        public static AccountRecoveryView CreateAccountRecoveryView(String id = "1")
        {
            return new AccountRecoveryView
            {
                Id = id,
                Email = id + "@tests.com"
            };
        }
        public static AccountResetView CreateAccountResetView(String id = "1")
        {
            return new AccountResetView
            {
                Id = id,

                Token = "Token" + id,
                NewPassword = "NewPassword1"
            };
        }
        public static AccountLoginView CreateAccountLoginView(String id = "1")
        {
            return new AccountLoginView
            {
                Id = id,
                Username = "Username" + id,
                Password = "Password1"
            };
        }
        public static AccountEditView CreateAccountEditView(String id = "1")
        {
            return new AccountEditView
            {
                Id = id,
                Username = "Username" + id,
                RoleName = "Name" + id
            };
        }

        public static Role CreateRole(String id = "1")
        {
            return new Role
            {
                Id = id,
                Name = "Name" + id
            };
        }
        public static RoleView CreateRoleView(String id = "1")
        {
            return new RoleView
            {
                Id = id,
                Name = "Name" + id
            };
        }

        public static RolePrivilege CreateRolePrivilege(String id = "1")
        {
            return new RolePrivilege
            {
                Id = id
            };
        }
        public static RolePrivilegeView CreateRolePrivilegeView(String id = "1")
        {
            return new RolePrivilegeView
            {
                Id = id
            };
        }

        public static Privilege CreatePrivilege(String id = "1")
        {
            return new Privilege
            {
                Id = id,
                Area = "Area" + id,
                Action = "Action" + id,
                Controller = "Controller" + id
            };
        }
        public static PrivilegeView CreatePrivilegeView(String id = "1")
        {
            return new PrivilegeView
            {
                Id = id,
                Area = "Area" + id,
                Action = "Action" + id,
                Controller = "Controller" + id
            };
        }

        public static TestModel CreateTestModel(String id = "1")
        {
            return new TestModel
            {
                Id = id,
                Text = "Text" + id
            };
        }
        public static TestView CreateTestView(String id = "1")
        {
            return new TestView
            {
                Id = id,
                Text = "Text" + id
            };
        }
    }
}
