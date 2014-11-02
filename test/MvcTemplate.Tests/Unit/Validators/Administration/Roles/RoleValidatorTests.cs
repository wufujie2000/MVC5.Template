using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.RoleView;
using MvcTemplate.Tests.Data;
using MvcTemplate.Validators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Validators
{
    [TestFixture]
    public class RoleValidatorTests
    {
        private RoleValidator validator;
        private TestingContext context;
        private Role role;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            validator = new RoleValidator(new UnitOfWork(context));
            validator.ModelState = new ModelStateDictionary();

            TearDownData();
            SetUpData();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
            validator.Dispose();
        }

        #region Method: CanCreate(RoleView view)

        [Test]
        public void CanCreate_CanNotCreateWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(validator.CanCreate(ObjectFactory.CreateRoleView()));
        }

        [Test]
        public void CanCreate_CanNotCreateWithAlreadyTakenRoleName()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.Name = role.Name.ToLower();
            roleView.Id += "1";

            Assert.IsFalse(validator.CanCreate(roleView));
        }

        [Test]
        public void CanCreate_AddsErrorMessageThenCanNotCreateWithAlreadyTakenRoleName()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.Name = role.Name.ToLower();
            roleView.Id += "OtherIdValue";
            validator.CanCreate(roleView);

            String actual = validator.ModelState["Name"].Errors[0].ErrorMessage;
            String expected = Validations.RoleNameIsAlreadyTaken;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCreate_CanCreateValidRole()
        {
            Assert.IsTrue(validator.CanCreate(ObjectFactory.CreateRoleView()));
        }

        #endregion

        #region Method: CanEdit(RoleView view)

        [Test]
        public void CanEdit_CanNotEditWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(validator.CanEdit(ObjectFactory.CreateRoleView()));
        }

        [Test]
        public void CanEdit_CanNotEditToAlreadyTakenRoleName()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.Name = role.Name.ToLower();
            roleView.Id += "1";

            Assert.IsFalse(validator.CanEdit(roleView));
        }

        [Test]
        public void CanEdit_AddsErrorMessageThenCanNotEditToAlreadyTakenRoleName()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.Name = role.Name.ToLower();
            roleView.Id += "OtherIdValue";
            validator.CanEdit(roleView);

            String actual = validator.ModelState["Name"].Errors[0].ErrorMessage;
            String expected = Validations.RoleNameIsAlreadyTaken;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanEditValidRole()
        {
            Assert.IsTrue(validator.CanEdit(ObjectFactory.CreateRoleView()));
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
                    RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege(privNumber++);
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
