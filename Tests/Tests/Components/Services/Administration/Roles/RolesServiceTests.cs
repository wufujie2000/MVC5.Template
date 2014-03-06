using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Template.Components.Services;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Services
{
    [TestFixture]
    public class RolesServiceTests
    {
        private Mock<RolesService> serviceMock;
        private RolesService service;
        private Context context;
        private Role role;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            serviceMock = new Mock<RolesService>(new UnitOfWork(context)) { CallBase = true };
            serviceMock.Object.HttpContext = new HttpContextBaseMock().HttpContextBase;
            serviceMock.Object.HttpContext.Request.RequestContext.RouteData.Values["language"] = "Abbreviation1";
            service = serviceMock.Object;

            TearDownData();
            SetUpData();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
            service.Dispose();
        }

        #region Method: GetView(String id)

        [Test]
        public void GetView_GetsViewbyId()
        {
            Assert.AreEqual(role.Id, service.GetView(role.Id).Id);
        }

        [Test]
        public void GetView_CallsSeedPrivilegesTree()
        {
            var roleView = service.GetView(role.Id);
            serviceMock.Verify(mock => mock.SeedPrivilegesTree(roleView), Times.Once());
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

            context = new TestingContext();
            var actual = context.Set<Role>().Find(expected.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [Test]
        public void Create_CreatesRolePrivileges()
        {
            context.Set<Role>().Remove(role);
            context.SaveChanges();
            
            var roleView = ObjectFactory.CreateRoleView();
            roleView.PrivilegesTree.SelectedIds = GetExpectedTree().SelectedIds;
            service.Create(roleView);

            context = new TestingContext();
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

            context = new TestingContext();
            var actual = context.Set<Role>().Find(expected.Id);

            Assert.AreEqual(expected.Name, actual.Name);
        }

        [Test]
        public void Edit_EditsRolePrivileges()
        {
            var roleView = service.GetView(role.Id);
            roleView.PrivilegesTree.SelectedIds = roleView.PrivilegesTree.SelectedIds.Take(1).ToList();
            service.Edit(roleView);

            context = new TestingContext();
            var expected = roleView.PrivilegesTree.SelectedIds;
            var actual = context.Set<Role>().Find(roleView.Id).RolePrivileges.Select(rolePriv => rolePriv.PrivilegeId).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_NullifiesDeletedRoleInUsers()
        {
            if (!context.Set<User>().Any(user => user.RoleId == role.Id))
                Assert.Inconclusive();

            service.Delete(role.Id);
            context = new TestingContext();

            Assert.IsFalse(context.Set<User>().Any(user => user.RoleId == role.Id));
        }

        [Test]
        public void Delete_DeletesRole()
        {
            if (context.Set<Role>().Find(role.Id) == null)
                Assert.Inconclusive();

            service.Delete(role.Id);
            context = new TestingContext();

            Assert.IsNull(context.Set<Role>().Find(role.Id));
        }

        #endregion

        #region Method: SeedPrivilegesTree(RoleView role)

        [Test]
        public void SeedPrivilegesTree_SeedsSelectedIds()
        {
            var expected = GetExpectedTree().SelectedIds;
            var actual = service.GetView(role.Id).PrivilegesTree.SelectedIds;

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void SeedPrivilegesTree_SeedsFirstLevelNodes()
        {
            var roleView = service.GetView(role.Id);

            var expected = GetExpectedTree().Nodes.GetEnumerator();
            var actual = roleView.PrivilegesTree.Nodes.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.Id, actual.Current.Id);
                Assert.AreEqual(expected.Current.Name, actual.Current.Name);
                Assert.AreEqual(expected.Current.Nodes.Count, actual.Current.Nodes.Count);
            }
        }

        [Test]
        public void SeedPrivilegesTree_SeedsSecondLevelNodes()
        {
            var roleView = service.GetView(role.Id);

            var expected = GetExpectedTree().Nodes.SelectMany(node => node.Nodes).GetEnumerator();
            var actual = roleView.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.Id, actual.Current.Id);
                Assert.AreEqual(expected.Current.Name, actual.Current.Name);
                Assert.AreEqual(expected.Current.Nodes.Count, actual.Current.Nodes.Count);
            }
        }

        [Test]
        public void SeedPrivilegesTree_SeedsThirdLevelNodes()
        {
            var roleView = service.GetView(role.Id);

            var expected = GetExpectedTree().Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();
            var actual = roleView.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.Id, actual.Current.Id);
                Assert.AreEqual(expected.Current.Name, actual.Current.Name);
                Assert.AreEqual(expected.Current.Nodes.Count, actual.Current.Nodes.Count);
            }
        }

        [Test]
        public void SeedPrivilegesTree_SeedsBranchesWithoutId()
        {
            var rootNode = service.GetView(role.Id).PrivilegesTree.Nodes.First();
            var branches = GetAllBranchNodes(rootNode);

            Assert.IsFalse(branches.Any(branch => branch.Id != null));
        }

        [Test]
        public void SeedPrivilegesTree_SeedsLeafsWithId()
        {
            var rootNode = service.GetView(role.Id).PrivilegesTree.Nodes.First();
            var leafs = GetAllLeafNodes(rootNode);

            Assert.IsFalse(leafs.Any(leaf => leaf.Id == null));
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

            for (Int32 number = 1; number <= 6; number++)
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
                if (number % 3 == 0)
                {
                    privilegeLanguage.Area = null;
                    privilegeLanguage.Controller = "C2";
                    privilegeLanguage.Action = "A" + (number / 3).ToString();
                }
                else
                {
                    privilegeLanguage.Area = "AR";
                    if (number % 2 == 0)
                        privilegeLanguage.Controller = "C1";
                    else
                        privilegeLanguage.Controller = "C2";
                }

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

        private Tree GetExpectedTree()
        {
            var expectedTree = new Tree();
            var rootNode = new TreeNode();
            expectedTree.Nodes.Add(rootNode);
            rootNode.Name = Template.Resources.Shared.Resources.AllPrivileges;
            expectedTree.SelectedIds = role.RolePrivileges.Select(rolePrivilege => rolePrivilege.PrivilegeId).ToArray();
            var languagePrivileges = context.Set<PrivilegeLanguage>().Where(privilege => privilege.Language.Abbreviation == service.CurrentLanguage);
            foreach (var areaPrivilege in languagePrivileges.GroupBy(privilege => privilege.Area).OrderBy(privilege => privilege.Key ?? privilege.FirstOrDefault().Controller))
            {
                TreeNode areaNode = new TreeNode(areaPrivilege.Key);
                foreach (var controllerPrivilege in areaPrivilege.GroupBy(privilege => privilege.Controller).OrderBy(privilege => privilege.Key))
                {
                    TreeNode controllerNode = new TreeNode(controllerPrivilege.Key);
                    foreach (var actionPrivilege in controllerPrivilege.GroupBy(privilege => privilege.Action).OrderBy(privilege => privilege.Key))
                        controllerNode.Nodes.Add(new TreeNode(actionPrivilege.First().PrivilegeId, actionPrivilege.Key));

                    if (areaNode.Name == null)
                        rootNode.Nodes.Add(controllerNode);
                    else
                        areaNode.Nodes.Add(controllerNode);
                }

                if (areaNode.Name != null)
                    rootNode.Nodes.Add(areaNode);
            }

            return expectedTree;
        }
        private List<TreeNode> GetAllBranchNodes(TreeNode root)
        {
            var branches = root.Nodes.Where(node => node.Nodes.Count > 0);
            foreach (var branch in branches.ToList())
                branches = branches.Union(GetAllBranchNodes(branch));

            if (root.Nodes.Count > 0)
                branches = branches.Union(new[] { root });

            return branches.ToList();
        }
        private List<TreeNode> GetAllLeafNodes(TreeNode root)
        {
            var leafs = root.Nodes.Where(node => node.Nodes.Count == 0);
            var branches = root.Nodes.Where(node => node.Nodes.Count > 0);
            foreach (var branch in branches.ToList())
                leafs = leafs.Union(GetAllLeafNodes(branch));

            if (root.Nodes.Count == 0)
                leafs = leafs.Union(new[] { root });

            return leafs.ToList();
        }

        #endregion
    }
}
