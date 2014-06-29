using AutoMapper;
using AutoMapper.QueryableExtensions;
using Moq;
using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Views.RoleView;
using MvcTemplate.Services;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Services
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

        #region Method: SeedPrivilegesTree(RoleView view)

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

            IEnumerator<JsTreeNode> expected = GetExpectedTree().Nodes.GetEnumerator();
            IEnumerator<JsTreeNode> actual = roleView.PrivilegesTree.Nodes.GetEnumerator();

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

            IEnumerator<JsTreeNode> expected = GetExpectedTree().Nodes.SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<JsTreeNode> actual = roleView.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).GetEnumerator();

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

            IEnumerator<JsTreeNode> expected = GetExpectedTree().Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<JsTreeNode> actual = roleView.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();

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
            JsTreeNode rootNode = service.GetView(role.Id).PrivilegesTree.Nodes.First();
            IEnumerable<JsTreeNode> branches = GetAllBranchNodes(rootNode);

            Assert.IsFalse(branches.Any(branch => branch.Id != null));
        }

        [Test]
        public void SeedPrivilegesTree_SeedsLeafsWithId()
        {
            JsTreeNode rootNode = service.GetView(role.Id).PrivilegesTree.Nodes.First();
            IEnumerable<JsTreeNode> leafs = GetAllLeafNodes(rootNode);

            Assert.IsFalse(leafs.Any(leaf => leaf.Id == null));
        }

        #endregion

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

        #region Method: GetViews()

        [Test]
        public void GetViews_GetsAccountViews()
        {
            IEnumerable<RoleView> actual = service.GetViews();
            IEnumerable<RoleView> expected = context
                .Set<Role>()
                .Project()
                .To<RoleView>()
                .OrderByDescending(account => account.EntityDate);

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: GetView(String id)

        [Test]
        public void GetView_GetsViewById()
        {
            Role roleInDatabase = context.Set<Role>().SingleOrDefault(model => model.Id == role.Id);
            RoleView expected = Mapper.Map<Role, RoleView>(roleInDatabase);
            RoleView actual = service.GetView(role.Id);

            TestHelper.PropertyWiseEquals(expected, actual);
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

            Role actual = context.Set<Role>().SingleOrDefault(model => model.Id == expected.Id);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [Test]
        public void Create_CreatesRolePrivileges()
        {
            RoleView roleView = ObjectFactory.CreateRoleView(2);
            roleView.PrivilegesTree.SelectedIds = GetExpectedTree().SelectedIds;
            service.Create(roleView);

            IEnumerable<String> expected = roleView.PrivilegesTree.SelectedIds;
            IEnumerable<String> actual = context.Set<Role>().SingleOrDefault(model => model.Id == roleView.Id).RolePrivileges.Select(r => r.PrivilegeId);

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
            Role actual = context.Set<Role>().SingleOrDefault(model => model.Id == expected.Id);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        [Test]
        public void Edit_EditsRolePrivileges()
        {
            RoleView roleView = service.GetView(role.Id);
            roleView.PrivilegesTree.SelectedIds = roleView.PrivilegesTree.SelectedIds.Take(1).ToList();
            service.Edit(roleView);

            IEnumerable<String> expected = roleView.PrivilegesTree.SelectedIds;
            IEnumerable<String> actual = context.Set<Role>().SingleOrDefault(model => model.Id == roleView.Id).RolePrivileges.Select(rolePriv => rolePriv.PrivilegeId).ToList();

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

            Assert.IsFalse(context.Set<Account>().Any(account => account.RoleId == role.Id));
        }

        [Test]
        public void Delete_DeletesRole()
        {
            if (context.Set<Role>().SingleOrDefault(model => model.Id == role.Id) == null)
                Assert.Inconclusive();

            service.Delete(role.Id);
            context = new TestingContext();

            Assert.IsNull(context.Set<Role>().SingleOrDefault(model => model.Id == role.Id));
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

        private JsTree GetExpectedTree()
        {
            JsTree expectedTree = new JsTree();
            JsTreeNode rootNode = new JsTreeNode();
            expectedTree.Nodes.Add(rootNode);
            rootNode.Name = MvcTemplate.Resources.Privilege.Titles.All;
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
                JsTreeNode areaNode = new JsTreeNode(areaPrivilege.Key);
                foreach (IGrouping<String, Privilege> controllerPrivilege in areaPrivilege.GroupBy(privilege => privilege.Controller).OrderBy(privilege => privilege.Key))
                {
                    JsTreeNode controllerNode = new JsTreeNode(controllerPrivilege.Key);
                    foreach (IGrouping<String, Privilege> actionPrivilege in controllerPrivilege.GroupBy(privilege => privilege.Action).OrderBy(privilege => privilege.Key))
                        controllerNode.Nodes.Add(new JsTreeNode(actionPrivilege.First().Id, actionPrivilege.Key));

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
        private List<JsTreeNode> GetAllBranchNodes(JsTreeNode root)
        {
            IEnumerable<JsTreeNode> branches = root.Nodes.Where(node => node.Nodes.Count > 0);
            foreach (JsTreeNode branch in branches.ToList())
                branches = branches.Union(GetAllBranchNodes(branch));

            if (root.Nodes.Count > 0)
                branches = branches.Union(new[] { root });

            return branches.ToList();
        }
        private List<JsTreeNode> GetAllLeafNodes(JsTreeNode root)
        {
            IEnumerable<JsTreeNode> leafs = root.Nodes.Where(node => node.Nodes.Count == 0);
            IEnumerable<JsTreeNode> branches = root.Nodes.Where(node => node.Nodes.Count > 0);
            foreach (JsTreeNode branch in branches.ToList())
                leafs = leafs.Union(GetAllLeafNodes(branch));

            if (root.Nodes.Count == 0)
                leafs = leafs.Union(new[] { root });

            return leafs.ToList();
        }

        #endregion
    }
}
