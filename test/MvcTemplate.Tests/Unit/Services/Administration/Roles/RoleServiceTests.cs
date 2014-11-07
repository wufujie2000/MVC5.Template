using AutoMapper;
using AutoMapper.QueryableExtensions;
using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Privilege;
using MvcTemplate.Services;
using MvcTemplate.Tests.Data;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MvcTemplate.Tests.Unit.Services
{
    [TestFixture]
    public class RoleServiceTests
    {
        private TestingContext context;
        private RoleService service;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            service = Substitute.ForPartsOf<RoleService>(new UnitOfWork(context));

            TearDownData();
        }

        [TearDown]
        public void TearDown()
        {
            Authorization.Provider = null;
            context.Dispose();
            service.Dispose();
        }

        #region Method: SeedPrivilegesTree(RoleView view)

        [Test]
        public void SeedPrivilegesTree_SeedsFirstLevelNodes()
        {
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerator<JsTreeNode> expected = CreateRoleView().PrivilegesTree.Nodes.GetEnumerator();
            IEnumerator<JsTreeNode> actual = role.PrivilegesTree.Nodes.GetEnumerator();

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
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerator<JsTreeNode> expected = CreateRoleView().PrivilegesTree.Nodes.SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<JsTreeNode> actual = role.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).GetEnumerator();

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
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerator<JsTreeNode> expected = CreateRoleView().PrivilegesTree.Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();
            IEnumerator<JsTreeNode> actual = role.PrivilegesTree.Nodes.SelectMany(node => node.Nodes).SelectMany(node => node.Nodes).GetEnumerator();

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
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerable<JsTreeNode> nodes = role.PrivilegesTree.Nodes;
            IEnumerable<JsTreeNode> branches = GetAllBranchNodes(nodes);

            CollectionAssert.IsEmpty(branches.Where(branch => branch.Id != null));
        }

        [Test]
        public void SeedPrivilegesTree_SeedsLeafsWithId()
        {
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView role = new RoleView();
            service.SeedPrivilegesTree(role);

            IEnumerable<JsTreeNode> nodes = role.PrivilegesTree.Nodes;
            IEnumerable<JsTreeNode> leafs = GetAllLeafNodes(nodes);

            CollectionAssert.IsEmpty(leafs.Where(leaf => leaf.Id == null));
        }

        #endregion

        #region Method: GetViews()

        [Test]
        public void GetViews_GetsRoleViews()
        {
            context.Set<Role>().Add(ObjectFactory.CreateRole(1));
            context.Set<Role>().Add(ObjectFactory.CreateRole(2));
            context.SaveChanges();

            IEnumerator<RoleView> actual = service.GetViews().GetEnumerator();
            IEnumerator<RoleView> expected = context
                .Set<Role>()
                .Project()
                .To<RoleView>()
                .OrderByDescending(view => view.CreationDate)
                .GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                CollectionAssert.AreEqual(expected.Current.PrivilegesTree.SelectedIds, actual.Current.PrivilegesTree.SelectedIds);
                Assert.AreEqual(expected.Current.CreationDate, actual.Current.CreationDate);
                Assert.AreEqual(expected.Current.Name, actual.Current.Name);
                Assert.AreEqual(expected.Current.Id, actual.Current.Id);
            }
        }

        #endregion

        #region Method: GetView(String id)

        [Test]
        public void GetView_GetsViewById()
        {
            service.When(sub => sub.SeedPrivilegesTree(Arg.Any<RoleView>())).DoNotCallBase();
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            RoleView expected = Mapper.Map<RoleView>(role);
            RoleView actual = service.GetView(role.Id);

            CollectionAssert.AreEqual(expected.PrivilegesTree.SelectedIds, actual.PrivilegesTree.SelectedIds);
            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void GetView_SeedsSelectedIds()
        {
            service.When(sub => sub.SeedPrivilegesTree(Arg.Any<RoleView>())).DoNotCallBase();
            Role role = CreateRoleWithPrivileges();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            IEnumerable expected = role.RolePrivileges.Select(rolePrivilege => rolePrivilege.PrivilegeId);
            IEnumerable actual = service.GetView(role.Id).PrivilegesTree.SelectedIds;

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void GetView_SeedsPrivilegesTree()
        {
            service.When(sub => sub.SeedPrivilegesTree(Arg.Any<RoleView>())).DoNotCallBase();
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            RoleView roleView = service.GetView(role.Id);

            service.Received().SeedPrivilegesTree(roleView);
        }

        #endregion

        #region Method: Create(RoleView view)

        [Test]
        public void Create_CreatesRole()
        {
            RoleView roleView = ObjectFactory.CreateRoleView();
            service.Create(roleView);

            Role actual = context.Set<Role>().SingleOrDefault();
            RoleView expected = roleView;

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void Create_CreatesRolePrivileges()
        {
            IEnumerable<Privilege> privileges = CreateRoleWithPrivileges().RolePrivileges.Select(rolePriv => rolePriv.Privilege);
            context.Set<Privilege>().AddRange(privileges);
            context.SaveChanges();

            RoleView roleView = CreateRoleView();
            service.Create(roleView);

            IEnumerable expected = privileges.Select(privilege => privilege.Id);
            IEnumerable actual = context
                .Set<Role>()
                .Single()
                .RolePrivileges
                .Select(role => role.PrivilegeId);

            CollectionAssert.AreEquivalent(expected, actual);
        }

        #endregion

        #region Method: Edit(RoleView view)

        [Test]
        public void Edit_EditsRole()
        {
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            RoleView roleView = Mapper.Map<RoleView>(role);
            roleView.Name += "EditedName";
            service.Edit(roleView);

            Role actual = context.Set<Role>().Single();
            RoleView expected = roleView;

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void Edit_EditsRolePrivileges()
        {
            Role role = CreateRoleWithPrivileges();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            RoleView roleView = CreateRoleView();
            roleView.PrivilegesTree.SelectedIds.RemoveAt(0);

            service.Edit(roleView);

            IEnumerable actual = context.Set<RolePrivilege>().Select(rolePriv => rolePriv.PrivilegeId);
            IEnumerable expected = CreateRoleView().PrivilegesTree.SelectedIds.Skip(1);

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void Edit_RefreshesAuthorizationProvider()
        {
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            service.Edit(ObjectFactory.CreateRoleView());

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_NullifiesDeletedRoleInAccounts()
        {
            Account account = ObjectFactory.CreateAccount();
            Role role = ObjectFactory.CreateRole();
            account.RoleId = role.Id;
            account.Role = role;

            context.Set<Account>().Add(account);
            context.SaveChanges();

            service.Delete(role.Id);

            CollectionAssert.IsNotEmpty(context.Set<Account>().Where(acc => acc.Id == account.Id && acc.RoleId == null));
        }

        [Test]
        public void Delete_DeletesRole()
        {
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            service.Delete(role.Id);

            CollectionAssert.IsEmpty(context.Set<Role>());
        }

        [Test]
        public void Delete_RefreshesAuthorizationProvider()
        {
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            service.Delete(role.Id);

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Test helpers

        private Role CreateRoleWithPrivileges()
        {
            String[] actions = { "Edit", "Delete" };
            String[] controllers = { "Roles", "Profile" };

            Int32 privNumber = 1;
            Role role = ObjectFactory.CreateRole();
            role.RolePrivileges = new List<RolePrivilege>();

            foreach (String controller in controllers)
                foreach (String action in actions)
                {
                    RolePrivilege rolePrivilege = ObjectFactory.CreateRolePrivilege(privNumber++);
                    rolePrivilege.Privilege = new Privilege { Controller = controller, Action = action };
                    rolePrivilege.Privilege.Area = controller != "Roles" ? "Administration" : null;
                    rolePrivilege.Privilege.Id = rolePrivilege.Id;
                    rolePrivilege.PrivilegeId = rolePrivilege.Id;
                    rolePrivilege.RoleId = role.Id;
                    rolePrivilege.Role = role;

                    role.RolePrivileges.Add(rolePrivilege);
                }

            return role;
        }
        private RoleView CreateRoleView()
        {
            Role role = CreateRoleWithPrivileges();
            RoleView roleView = Mapper.Map<RoleView>(role);
            roleView.PrivilegesTree = CreatePrivilegesTree(role);

            return roleView;
        }
        private JsTree CreatePrivilegesTree(Role role)
        {
            JsTree expectedTree = new JsTree();
            expectedTree.Nodes.Add(new JsTreeNode());
            JsTreeNode rootNode = expectedTree.Nodes[0];

            rootNode.Name = Titles.All;
            expectedTree.SelectedIds = role.RolePrivileges.Select(rolePrivilege => rolePrivilege.PrivilegeId).ToList();

            IEnumerable<Privilege> allPrivileges = role
                .RolePrivileges
                .Select(rolePriv => rolePriv.Privilege)
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
        private void TearDownData()
        {
            context.Set<Privilege>().RemoveRange(context.Set<Privilege>());
            context.Set<Account>().RemoveRange(context.Set<Account>());
            context.Set<Role>().RemoveRange(context.Set<Role>());
            context.SaveChanges();
        }

        private IEnumerable<JsTreeNode> GetAllBranchNodes(IEnumerable<JsTreeNode> nodes)
        {
            List<JsTreeNode> branches = nodes.Where(node => node.Nodes.Count > 0).ToList();
            foreach (JsTreeNode branch in branches.ToArray())
                branches.AddRange(GetAllBranchNodes(branch.Nodes));

            return branches;
        }
        private IEnumerable<JsTreeNode> GetAllLeafNodes(IEnumerable<JsTreeNode> nodes)
        {
            List<JsTreeNode> leafs = nodes.Where(node => node.Nodes.Count == 0).ToList();
            IEnumerable<JsTreeNode> branches = nodes.Where(node => node.Nodes.Count > 0);

            foreach (JsTreeNode branch in branches)
                leafs.AddRange(GetAllLeafNodes(branch.Nodes));

            return leafs;
        }

        #endregion
    }
}
