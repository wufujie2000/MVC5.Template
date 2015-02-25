using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.RoleView;
using MvcTemplate.Tests.Data;
using MvcTemplate.Validators;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Validators
{
    public class RoleValidatorTests : IDisposable
    {
        private RoleValidator validator;
        private TestingContext context;
        private Role role;

        public RoleValidatorTests()
        {
            context = new TestingContext();
            validator = new RoleValidator(new UnitOfWork(context));
            validator.ModelState = new ModelStateDictionary();

            TearDownData();
            SetUpData();
        }
        public void Dispose()
        {
            context.Dispose();
            validator.Dispose();
        }

        #region Method: CanCreate(RoleView view)

        [Fact]
        public void CanCreate_CanNotCreateWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanCreate(ObjectFactory.CreateRoleView()));
        }

        [Fact]
        public void CanCreate_CanNotCreateWithAlreadyUsedRoleName()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.Name = role.Name.ToLower();
            roleView.Id += "1";

            Assert.False(validator.CanCreate(roleView));
        }

        [Fact]
        public void CanCreate_AddsErrorMessageThenCanNotCreateWithAlreadyUsedRoleName()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.Name = role.Name.ToLower();
            roleView.Id += "OtherIdValue";
            validator.CanCreate(roleView);

            String actual = validator.ModelState["Name"].Errors[0].ErrorMessage;
            String expected = Validations.RoleNameIsAlreadyUsed;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCreate_CanCreateValidRole()
        {
            Assert.True(validator.CanCreate(ObjectFactory.CreateRoleView()));
        }

        #endregion

        #region Method: CanEdit(RoleView view)

        [Fact]
        public void CanEdit_CanNotEditWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanEdit(ObjectFactory.CreateRoleView()));
        }

        [Fact]
        public void CanEdit_CanNotEditToAlreadyUsedRoleName()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.Name = role.Name.ToLower();
            roleView.Id += "1";

            Assert.False(validator.CanEdit(roleView));
        }

        [Fact]
        public void CanEdit_AddsErrorMessageThenCanNotEditToAlreadyUsedRoleName()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.Name = role.Name.ToLower();
            roleView.Id += "OtherIdValue";
            validator.CanEdit(roleView);

            String actual = validator.ModelState["Name"].Errors[0].ErrorMessage;
            String expected = Validations.RoleNameIsAlreadyUsed;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEdit_CanEditValidRole()
        {
            Assert.True(validator.CanEdit(ObjectFactory.CreateRoleView()));
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            Account account = ObjectFactory.CreateAccount();
            role = ObjectFactory.CreateRole();
            account.RoleId = role.Id;

            context.Set<Account>().Add(account);

            role.RolePrivileges = new List<RolePrivilege>();

            Int32 privNumber = 1;
            IEnumerable<String> controllers = new[] { "Accounts", "Roles" };
            IEnumerable<String> actions = new[] { "Index", "Create", "Details", "Edit", "Delete" };

            foreach (String controller in controllers)
                foreach (String action in actions)
                {
                    RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege((privNumber++).ToString());
                    rolePrivilege.Privilege = new Privilege { Area = "Administration", Controller = controller, Action = action };
                    rolePrivilege.Privilege.Id = rolePrivilege.Id;
                    rolePrivilege.PrivilegeId = rolePrivilege.Id;
                    rolePrivilege.RoleId = role.Id;
                    rolePrivilege.Role = role;

                    role.RolePrivileges.Add(rolePrivilege);
                }

            context.Set<Role>().Add(role);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            context.Set<Privilege>().RemoveRange(context.Set<Privilege>());
            context.Set<Account>().RemoveRange(context.Set<Account>());
            context.Set<Role>().RemoveRange(context.Set<Role>());
            context.SaveChanges();
        }

        #endregion
    }
}
