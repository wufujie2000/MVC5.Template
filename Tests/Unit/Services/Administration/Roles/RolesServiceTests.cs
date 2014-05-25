using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Template.Data.Core;
using Template.Objects;
using Template.Resources;
using Template.Resources.Views.RoleView;
using Template.Services;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Services
{
    [TestFixture]
    public class RolesServiceTests
    {
        private RolesService service;
        private Context context;
        private Role role;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            service = new RolesService(new UnitOfWork(context));
            service.ModelState = new ModelStateDictionary();

            TearDownData();
            SetUpData();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
            service.Dispose();
        }

        #region Method: CanCreate(RoleView view)

        [Test]
        public void CanCreate_CanNotCreateWithInvalidModelState()
        {
            service.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(service.CanCreate(ObjectFactory.CreateRoleView()));
        }

        [Test]
        public void CanCreate_CanNotCreateWithAlreadyTakenRoleName()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.Name = role.Name.ToLower();
            roleView.Id += "1";

            Assert.IsFalse(service.CanCreate(roleView));
        }

        [Test]
        public void CanCreate_AddsErrorMessageThenCanNotCreateWithAlreadyTakenRoleName()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.Name = role.Name.ToLower();
            roleView.Id += "OtherIdValue";
            service.CanCreate(roleView);

            String expected = Validations.RoleNameIsAlreadyTaken;
            String actual = service.ModelState["Name"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCreate_CanCreateValidRole()
        {
            Assert.IsTrue(service.CanCreate(ObjectFactory.CreateRoleView()));
        }

        #endregion

        #region Method: CanEdit(RoleView view)

        [Test]
        public void CanEdit_CanNotEditWithInvalidModelState()
        {
            service.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(service.CanEdit(ObjectFactory.CreateRoleView()));
        }

        [Test]
        public void CanEdit_CanNotEditToAlreadyTakenRoleName()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.Name = role.Name.ToLower();
            roleView.Id += "1";

            Assert.IsFalse(service.CanEdit(roleView));
        }

        [Test]
        public void CanEdit_AddsErrorMessageThenCanNotEditToAlreadyTakenRoleName()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.Name = role.Name.ToLower();
            roleView.Id += "OtherIdValue";
            service.CanEdit(roleView);

            String expected = Validations.RoleNameIsAlreadyTaken;
            String actual = service.ModelState["Name"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanEditValidRole()
        {
            Assert.IsTrue(service.CanEdit(ObjectFactory.CreateRoleView()));
        }

        #endregion

        #region Method: GetView(String id)

        [Test]
        public void GetView_GetsViewById()
        {
            Assert.AreEqual(role.Id, service.GetView(role.Id).Id);
        }

        [Test]
        public void GetView_SeedsPrivilegesTree()
        {
            Mock<RolesService> serviceMock = new Mock<RolesService>(new UnitOfWork(context)) { CallBase = true };
            service = serviceMock.Object;

            RoleView roleView = service.GetView(role.Id);

            serviceMock.Verify(mock => mock.SeedPrivilegesTree(roleView), Times.Once());
        }

        #endregion

        #region Method: Create(RoleView view)

        [Test]
        public void Create_CreatesRole()
        {
            TearDownData();

            RoleView expected = ObjectFactory.CreateRoleView();
            service.Create(expected);

            context = new TestingContext();
            Role actual = context.Set<Role>().Find(expected.Id);

            Assert.AreEqual(expected.Name, actual.Name);
        }

        [Test]
        public void Create_CreatesRolePrivileges()
        {
            TearDownData();

            RoleView roleView = ObjectFactory.CreateRoleView();
            roleView.PrivilegesTree.SelectedIds = GetExpectedTree().SelectedIds;
            service.Create(roleView);

            context = new TestingContext();
            IEnumerable<String> expected = roleView.PrivilegesTree.SelectedIds;
            IEnumerable<String> actual = context.Set<Role>().Find(roleView.Id).RolePrivileges.Select(r => r.PrivilegeId);
            
            CollectionAssert.AreEquivalent(expected, actual);
        }

        #endregion

        #region Method: Edit(RoleView view)

        [Test]
        public void Edit_EditsRole()
        {
            RoleView expected = service.GetView(role.Id);
            expected.Name += "EditedName"; 
            service.Edit(expected);

            context = new TestingContext();
            Role actual = context.Set<Role>().Find(expected.Id);

            Assert.AreEqual(expected.Name, actual.Name);
        }

        [Test]
        public void Edit_EditsRolePrivileges()
        {
            RoleView roleView = service.GetView(role.Id);
            roleView.PrivilegesTree.SelectedIds = roleView.PrivilegesTree.SelectedIds.Take(1).ToList();
            service.Edit(roleView);

            context = new TestingContext();
            IEnumerable<String> expected = roleView.PrivilegesTree.SelectedIds;
            IEnumerable<String> actual = context.Set<Role>().Find(roleView.Id).RolePrivileges.Select(rolePriv => rolePriv.PrivilegeId).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_NullifiesDeletedRoleInAccounts()
        {
            if (!context.Set<Account>().Any(account => account.RoleId == role.Id))
                Assert.Inconclusive();

            service.Delete(role.Id);
            context = new TestingContext();

            Assert.IsFalse(context.Set<Account>().Any(account => account.RoleId == role.Id));
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
            IEnumerable<String> expected = GetExpectedTree().SelectedIds;
            IEnumerable<String> actual = service.GetView(role.Id).PrivilegesTree.SelectedIds;

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void SeedPrivilegesTree_SeedsFirstLevelNodes()
        {
            RoleView roleView = service.GetView(role.Id);

            IEnumerator<TreeNode> expected = GetExpectedTree().Nodes.GetEnumerator();
            IEnumerator<TreeNode> actual = roleView.PrivilegesTree.Nodes.GetEnumerator();

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
            RoleView roleView = service.GetView(role.Id);

            IEnumerator<TreeNode> expected = GetExpectedTree().Nodes.SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<TreeNode> actual = roleView.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).GetEnumerator();

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
            RoleView roleView = service.GetView(role.Id);

            IEnumerator<TreeNode> expected = GetExpectedTree().Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<TreeNode> actual = roleView.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();

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
            TreeNode rootNode = service.GetView(role.Id).PrivilegesTree.Nodes.First();
            IEnumerable<TreeNode> branches = GetAllBranchNodes(rootNode);

            Assert.IsFalse(branches.Any(branch => branch.Id != null));
        }

        [Test]
        public void SeedPrivilegesTree_SeedsLeafsWithId()
        {
            TreeNode rootNode = service.GetView(role.Id).PrivilegesTree.Nodes.First();
            IEnumerable<TreeNode> leafs = GetAllLeafNodes(rootNode);

            Assert.IsFalse(leafs.Any(leaf => leaf.Id == null));
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
            context.Set<Privilege>().RemoveRange(context.Set<Privilege>().Where(privilege => privilege.Id.StartsWith(ObjectFactory.TestId)));
            context.Set<Language>().RemoveRange(context.Set<Language>().Where(language => language.Id.StartsWith(ObjectFactory.TestId)));
            context.Set<Account>().RemoveRange(context.Set<Account>().Where(account => account.Id.StartsWith(ObjectFactory.TestId)));
            context.Set<Role>().RemoveRange(context.Set<Role>().Where(role => role.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        private Tree GetExpectedTree()
        {
            Tree expectedTree = new Tree();
            TreeNode rootNode = new TreeNode();
            expectedTree.Nodes.Add(rootNode);
            rootNode.Name = Template.Resources.Privilege.Titles.All;
            expectedTree.SelectedIds = role.RolePrivileges.Select(rolePrivilege => rolePrivilege.PrivilegeId).ToArray();
            
            IEnumerable<Privilege> allPrivileges = context.Set<Privilege>().ToList()
                .Select(privilege => new Privilege
                    {
                        Id = privilege.Id,
                        Area = ResourceProvider.GetPrivilegeAreaTitle(privilege.Area),
                        Action = ResourceProvider.GetPrivilegeActionTitle(privilege.Action),
                        Controller = ResourceProvider.GetPrivilegeControllerTitle(privilege.Controller)
                    });

            foreach (IGrouping<String, Privilege> areaPrivilege in allPrivileges.GroupBy(privilege => privilege.Area).OrderBy(privilege => privilege.Key ?? privilege.FirstOrDefault().Controller))
            {
                TreeNode areaNode = new TreeNode(areaPrivilege.Key);
                foreach (IGrouping<String, Privilege> controllerPrivilege in areaPrivilege.GroupBy(privilege => privilege.Controller).OrderBy(privilege => privilege.Key))
                {
                    TreeNode controllerNode = new TreeNode(controllerPrivilege.Key);
                    foreach (IGrouping<String, Privilege> actionPrivilege in controllerPrivilege.GroupBy(privilege => privilege.Action).OrderBy(privilege => privilege.Key))
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
            IEnumerable<TreeNode> branches = root.Nodes.Where(node => node.Nodes.Count > 0);
            foreach (TreeNode branch in branches.ToList())
                branches = branches.Union(GetAllBranchNodes(branch));

            if (root.Nodes.Count > 0)
                branches = branches.Union(new[] { root });

            return branches.ToList();
        }
        private List<TreeNode> GetAllLeafNodes(TreeNode root)
        {
            IEnumerable<TreeNode> leafs = root.Nodes.Where(node => node.Nodes.Count == 0);
            IEnumerable<TreeNode> branches = root.Nodes.Where(node => node.Nodes.Count > 0);
            foreach (TreeNode branch in branches.ToList())
                leafs = leafs.Union(GetAllLeafNodes(branch));

            if (root.Nodes.Count == 0)
                leafs = leafs.Union(new[] { root });

            return leafs.ToList();
        }

        #endregion
    }
}
