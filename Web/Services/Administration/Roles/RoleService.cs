using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcTemplate.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public virtual void SeedPrivilegesTree(RoleView view)
        {
            JsTreeNode rootNode = new JsTreeNode();
            view.PrivilegesTree.Nodes.Add(rootNode);
            rootNode.Name = Resources.Privilege.Titles.All;

            IEnumerable<IGrouping<String, Privilege>> allPrivileges = UnitOfWork
                .Repository<Privilege>().ToList()
                .Select(privilege => new Privilege
                {
                    Id = privilege.Id,
                    Area = ResourceProvider.GetPrivilegeAreaTitle(privilege.Area),
                    Action = ResourceProvider.GetPrivilegeActionTitle(privilege.Action),
                    Controller = ResourceProvider.GetPrivilegeControllerTitle(privilege.Controller)
                })
                .GroupBy(privilege => privilege.Area)
                .OrderBy(privilege => privilege.Key ?? privilege.FirstOrDefault().Controller);

            foreach (IGrouping<String, Privilege> areaPrivilege in allPrivileges)
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
        }

        public IEnumerable<RoleView> GetViews()
        {
            return UnitOfWork
                .Repository<Role>()
                .ProjectTo<RoleView>()
                .OrderByDescending(view => view.EntityDate);
        }
        public RoleView GetView(String id)
        {
            RoleView role = UnitOfWork.Repository<Role>().GetById<RoleView>(id);
            role.PrivilegesTree.SelectedIds = UnitOfWork
                .Repository<RolePrivilege>()
                .Where(rolePrivilege => rolePrivilege.RoleId == role.Id)
                .Select(rolePrivilege => rolePrivilege.PrivilegeId)
                .ToList();

            SeedPrivilegesTree(role);

            return role;
        }

        public void Create(RoleView view)
        {
            CreateRole(view);
            CreateRolePrivileges(view);
            UnitOfWork.Commit();
        }
        public void Edit(RoleView view)
        {
            EditRole(view);
            DeleteRolePrivileges(view);
            CreateRolePrivileges(view);
            UnitOfWork.Commit();
        }
        public void Delete(String id)
        {
            RemoveRoleFromAccounts(id);
            UnitOfWork.Repository<Role>().Delete(id);
            UnitOfWork.Commit();
        }

        private void CreateRole(RoleView view)
        {
            Role model = UnitOfWork.ToModel<RoleView, Role>(view);
            UnitOfWork.Repository<Role>().Insert(model);
        }
        private void EditRole(RoleView view)
        {
            Role model = UnitOfWork.ToModel<RoleView, Role>(view);
            UnitOfWork.Repository<Role>().Update(model);
        }

        private void DeleteRolePrivileges(RoleView view)
        {
            IQueryable<String> rolePrivileges = UnitOfWork.Repository<RolePrivilege>()
                .Where(rolePrivilege => rolePrivilege.RoleId == view.Id)
                .Select(rolePrivilege => rolePrivilege.Id);

            foreach (String rolePrivilege in rolePrivileges)
                UnitOfWork.Repository<RolePrivilege>().Delete(rolePrivilege);
        }
        private void CreateRolePrivileges(RoleView view)
        {
            foreach (String privilegeId in view.PrivilegesTree.SelectedIds)
                UnitOfWork.Repository<RolePrivilege>().Insert(new RolePrivilege()
                {
                    RoleId = view.Id,
                    PrivilegeId = privilegeId
                });
        }
        private void RemoveRoleFromAccounts(String roleId)
        {
            IQueryable<Account> accountsWithRole = UnitOfWork
                .Repository<Account>()
                .Where(account => account.RoleId == roleId);

            foreach (Account account in accountsWithRole)
            {
                account.RoleId = null;
                UnitOfWork.Repository<Account>().Update(account);
            }
        }
    }
}
