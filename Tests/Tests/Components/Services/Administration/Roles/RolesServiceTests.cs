using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Template.Components.Services;
using Template.Data.Core;
using Template.Objects;
using Template.Resources;
using Template.Tests.Data;
using Template.Tests.Helpers;

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
        public void Delete_NullifiesDeletedRoleInPerson()
        {
            if (!context.Set<Person>().Any(person => person.RoleId == role.Id))
                Assert.Inconclusive();

            service.Delete(role.Id);
            context = new TestingContext();

            Assert.IsFalse(context.Set<Person>().Any(person => person.RoleId == role.Id));
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
            account.Person = ObjectFactory.CreatePerson();
            account.PersonId = account.Person.Id;

            role = ObjectFactory.CreateRole();
            account.Person.RoleId = role.Id;
            context.Set<Account>().Add(account);

            role.RolePrivileges = new List<RolePrivilege>();

            var privNumber = 1;
            var controllers = new[] { "Users", "Roles" };
            var actions = new[] { "Index", "Create", "Details", "Edit", "Delete" };

            foreach (var controller in controllers)
                foreach (var action in actions)
                {
                    var rolePrivilege = ObjectFactory.CreateRolePrivilege(privNumber++);
                    rolePrivilege.Privilege = new Privilege() { Area = "Administration", Controller = controller, Action = action };
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
            foreach (var person in context.Set<Person>().Where(person => person.Id.StartsWith(ObjectFactory.TestId)))
                context.Set<Person>().Remove(person);

            foreach (var role in context.Set<Role>().Where(role => role.Id.StartsWith(ObjectFactory.TestId)))
                context.Set<Role>().Remove(role);

            foreach (var privilege in context.Set<Privilege>().Where(privilege => privilege.Id.StartsWith(ObjectFactory.TestId)))
                context.Set<Privilege>().Remove(privilege);

            foreach (var language in context.Set<Language>().Where(language => language.Id.StartsWith(ObjectFactory.TestId)))
                context.Set<Language>().Remove(language);

            context.SaveChanges();
        }

        private Tree GetExpectedTree()
        {
            var expectedTree = new Tree();
            var rootNode = new TreeNode();
            expectedTree.Nodes.Add(rootNode);
            rootNode.Name = Template.Resources.Privilege.Titles.All;
            expectedTree.SelectedIds = role.RolePrivileges.Select(rolePrivilege => rolePrivilege.PrivilegeId).ToArray();
            var allPrivileges = context.Set<Privilege>().ToList().Select(privilege => new
            {
                Id = privilege.Id,
                Area = ResourceProvider.GetPrivilegeAreaTitle(privilege.Area),
                Action = ResourceProvider.GetPrivilegeActionTitle(privilege.Action),
                Controller = ResourceProvider.GetPrivilegeControllerTitle(privilege.Controller)
            });
            foreach (var areaPrivilege in allPrivileges.GroupBy(privilege => privilege.Area).OrderBy(privilege => privilege.Key ?? privilege.FirstOrDefault().Controller))
            {
                TreeNode areaNode = new TreeNode(areaPrivilege.Key);
                foreach (var controllerPrivilege in areaPrivilege.GroupBy(privilege => privilege.Controller).OrderBy(privilege => privilege.Key))
                {
                    TreeNode controllerNode = new TreeNode(controllerPrivilege.Key);
                    foreach (var actionPrivilege in controllerPrivilege.GroupBy(privilege => privilege.Action).OrderBy(privilege => privilege.Key))
                        controllerNode.Nodes.Add(new TreeNode(actionPrivilege.First().Id, actionPrivilege.Key));

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
