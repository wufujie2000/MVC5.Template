using System;
using System.Collections.Generic;
using System.Linq;
using Template.Components.Extensions.Html;
using Template.Components.Extensions.Mvc;
using Template.Data.Core;
using Template.Objects;
using Template.Resources;
using Template.Resources.Views.RoleView;

namespace Template.Services
{
    public class RolesService : GenericService<Role, RoleView>, IRolesService
    {
        public RolesService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public virtual void SeedPrivilegesTree(RoleView view)
        {
            JsTreeNode rootNode = new JsTreeNode();
            view.PrivilegesTree.Nodes.Add(rootNode);
            rootNode.Name = Resources.Privilege.Titles.All;

            IEnumerable<IGrouping<String, Privilege>> allPrivileges = UnitOfWork
                .Repository<Privilege>()
                .Query()
                .ToList()
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

        public Boolean CanCreate(RoleView view)
        {
            Boolean isValid = ModelState.IsValid;
            isValid &= IsUniqueRole(view);

            return isValid;
        }
        public Boolean CanEdit(RoleView view)
        {
            Boolean isValid = ModelState.IsValid;
            isValid &= IsUniqueRole(view);

            return isValid;
        }

        public override RoleView GetView(String id)
        {
            RoleView role = base.GetView(id);
            role.PrivilegesTree.SelectedIds = UnitOfWork
                .Repository<RolePrivilege>()
                .Query(rolePrivilege => rolePrivilege.RoleId == role.Id)
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

        private Boolean IsUniqueRole(RoleView view)
        {
            Boolean isUnique = !UnitOfWork.Repository<Role>()
                .Query(role =>
                    role.Id != view.Id &&
                    role.Name.ToUpper() == view.Name.ToUpper())
                .Any();

            if (!isUnique)
                ModelState.AddModelError<RoleView>(model => model.Name, Validations.RoleNameIsAlreadyTaken);

            return isUnique;
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
                .Query(rolePrivilege => rolePrivilege.RoleId == view.Id)
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
            IQueryable<Account> accountsWithRole = UnitOfWork.Repository<Account>()
                .Query(account => account.RoleId == roleId);

            foreach (Account account in accountsWithRole)
            {
                account.RoleId = null;
                UnitOfWork.Repository<Account>().Update(account);
            }
        }
    }
}
