using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template.Components.Services;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Helpers;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Services
{
    [TestFixture]
    public class RolesServiceTests
    {
        private ModelStateDictionary modelState;
        private RolesService service;
        private Context context;
        private Role role;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpContextStub().Context;
            HttpContext.Current.Request.RequestContext.RouteData.Values["language"] = "Abbreviation";

            modelState = new ModelStateDictionary();
            service = new RolesService(modelState);
            context = new Context();

            SetUpData();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
            TearDownData();

            service.Dispose();
            context.Dispose();
        }

        #region Method: GetView(String id)

        [Test]
        public void GetView_GetsViewbyId()
        {
            Assert.AreEqual(role.Id, service.GetView(role.Id).Id);
        }

        [Test]
        public void GetView_SeedsPrivilegeTreeSelectedIds()
        {
            var roleView = service.GetView(role.Id);
            var expecteIds = new List<String>();
            expecteIds.Add(TestContext.CurrentContext.Test.Name + "1");
            expecteIds.Add(TestContext.CurrentContext.Test.Name + "2");

            CollectionAssert.AreEqual(expecteIds, roleView.PrivilegesTree.SelectedIds);
        }

        [Test]
        public void GetView_SeedsPrivilegeTreeRootNode()
        {
            var roleView = service.GetView(role.Id);
            var rootNode = roleView.PrivilegesTree.Nodes.First();

            Assert.AreEqual(Template.Resources.Shared.Resources.AllPrivileges, rootNode.Name);
            Assert.AreEqual(1, roleView.PrivilegesTree.Nodes.Count);
            Assert.AreEqual(String.Empty, rootNode.Id);
            Assert.Greater(rootNode.Nodes.Count, 0);
        }

        [Test]
        public void GetView_SeedsPrivilegeTreeRootNodeChildren()
        {
            var roleView = service.GetView(role.Id);
            var rootNode = roleView.PrivilegesTree.Nodes.First();

            var firstChild = rootNode.Nodes[0];
            var secondChild = rootNode.Nodes[1];

            Assert.AreEqual(role.RolePrivileges.First().Privilege.PrivilegeLanguages.First().Area, firstChild.Name);
        }

        #endregion

        #region Method: Create(RoleView view)

        [Test]
        public void Create_CreatesRole()
        {
            context.Set<Role>().Remove(role);
            context.SaveChanges();

            var expected = ObjectFactory.CreateRoleView();
            service.Create(expected);
            context = new Context();

            var actual = context.Set<Role>().Find(expected.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [Test]
        public void Create_CreatesRolePrivileges()
        {
            context.Set<Role>().Remove(role);
            context.SaveChanges();
            
            var roleView = ObjectFactory.CreateRoleView();
            roleView.PrivilegesTree.SelectedIds = context.Set<Privilege>().Where(privilege => privilege.Id.StartsWith(role.Id)).Select(privilege => privilege.Id).ToList();
            service.Create(roleView);
            context = new Context();

            var expected = roleView.PrivilegesTree.SelectedIds;
            var actual = context.Set<Role>().Find(roleView.Id).RolePrivileges.Select(r => r.PrivilegeId);
            
            CollectionAssert.AreEquivalent(expected, actual);
        }

        #endregion

        #region Method: Edit(RoleView view)

        [Test]
        public void Edit_EditsRole()
        {
            var expected = service.GetView(role.Id);
            expected.Name = "EditedName"; 
            service.Edit(expected);
            context = new Context();

            var actual = context.Set<Role>().Find(expected.Id);

            Assert.AreEqual(expected.Name, actual.Name);
        }

        [Test]
        public void Edit_EditsRolePrivileges()
        {
            var roleView = service.GetView(role.Id);
            roleView.PrivilegesTree.SelectedIds = context.Set<Privilege>().Where(privilege => privilege.Id.StartsWith(role.Id)).Take(1).Select(privilege => privilege.Id).ToList();
            service.Edit(roleView);
            context = new Context();

            var expected = roleView.PrivilegesTree.SelectedIds.GetEnumerator();
            var actual = context.Set<Role>().Find(roleView.Id).RolePrivileges.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
                Assert.AreEqual(expected.Current, actual.Current.PrivilegeId);
        }

        #endregion

        #region Method: Method: Delete(String id)

        [Test]
        public void Delete_RemovedRoleFromUsers()
        {
            service.Delete(role.Id);
            context = new Context();

            Assert.IsFalse(context.Set<User>().Any(user => user.RoleId == role.Id));
        }

        [Test]
        public void Delete_DeletesRole()
        {
            service.Delete(role.Id);
            context = new Context();

            Assert.IsNull(context.Set<Role>().Find(role.Id));
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            var account = ObjectFactory.CreateAccount();
            account.User = ObjectFactory.CreateUser();
            account.UserId = account.User.Id;

            role = ObjectFactory.CreateRole();
            account.User.RoleId = role.Id;
            context.Set<Account>().Add(account);

            role.RolePrivileges = new List<RolePrivilege>();
            var language = ObjectFactory.CreateLanguage();

            for (Int32 number = 1; number <= 2; number++)
            {
                var privilegeLanguage = ObjectFactory.CreatePrivilegeLanguage(number);
                var rolePrivilege = ObjectFactory.CreateRolePrivilege(number);
                var privilege = ObjectFactory.CreatePrivilege(number);
                rolePrivilege.PrivilegeId = privilege.Id;
                rolePrivilege.Privilege = privilege;
                rolePrivilege.RoleId = role.Id;
                rolePrivilege.Role = role;

                privilegeLanguage.PrivilegeId = privilege.Id;
                privilegeLanguage.LanguageId = language.Id;
                privilegeLanguage.Privilege = privilege;
                privilegeLanguage.Language = language;
                if (number % 2 == 0)
                    privilegeLanguage.Area = null;

                context.Set<PrivilegeLanguage>().Add(privilegeLanguage);
                role.RolePrivileges.Add(rolePrivilege);
            }

            context.Set<Role>().Add(role);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            var testId = TestContext.CurrentContext.Test.Name;
            foreach (var user in context.Set<User>().Where(user => user.Id.StartsWith(testId)))
                context.Set<User>().Remove(user);

            foreach (var role in context.Set<Role>().Where(role => role.Id.StartsWith(testId)))
                context.Set<Role>().Remove(role);

            foreach (var privilege in context.Set<Privilege>().Where(privilege => privilege.Id.StartsWith(testId)))
                context.Set<Privilege>().Remove(privilege);

            foreach (var language in context.Set<Language>().Where(language => language.Id.StartsWith(testId)))
                context.Set<Language>().Remove(language);

            context.SaveChanges();
        }

        #endregion
    }
}
