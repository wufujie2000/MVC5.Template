using Template.Objects;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Template.Components.Services
{
    public class RolesService : GenericService<Role, RoleView>
    {
        public RolesService(ModelStateDictionary modelState)
            : base(modelState)
        {
        }

        public override RoleView GetView(String id)
        {
            RoleView role = base.GetView(id);
            SeedPrivilegesTree(role);

            return role;
        }

        public override void Create(RoleView view)
        {
            CreateRole(view);
            CreateRolePrivileges(view);
            UnitOfWork.Commit();
        }
        public override void Edit(RoleView view)
        {
            EditRole(view);
            DeleteRolePrivileges(view);
            CreateRolePrivileges(view);
            UnitOfWork.Commit();
        }
        public override void Delete(String id)
        {
            RemoveRoleFromUsers(id);
            UnitOfWork.Repository<Role>().Delete(id);
            UnitOfWork.Commit();
        }

        public void SeedPrivilegesTree(RoleView role)
        {
            var rootNode = new TreeNode();
            role.PrivilegesTree = new Tree();
            role.PrivilegesTree.Nodes.Add(rootNode);
            rootNode.Name = Resources.Shared.Resources.AllPrivileges;
            role.PrivilegesTree.SelectedIds = role.RolePrivileges.Select(rolePrivilege => rolePrivilege.PrivilegeId).ToArray();
            var allPrivileges = UnitOfWork.Repository<PrivilegeLanguage>().Query(privilege => privilege.Language.Abbreviation == CurrentLanguage);
            foreach (var areaPrivilege in allPrivileges.GroupBy(privilege => privilege.Area).OrderBy(privilege => privilege.Key ?? privilege.FirstOrDefault().Controller))
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
        }

        private void CreateRole(RoleView view)
        {
            var model = UnitOfWork.ToModel<RoleView, Role>(view);
            UnitOfWork.Repository<Role>().Insert(model);
        }
        private void EditRole(RoleView view)
        {
            var model = UnitOfWork.ToModel<RoleView, Role>(view);
            UnitOfWork.Repository<Role>().Update(model);
        }

        private void DeleteRolePrivileges(RoleView view)
        {
            var rolePrivileges = UnitOfWork
                .Repository<RolePrivilege>()
                .Query(rolePrivilege => rolePrivilege.RoleId == view.Id)
                .Select(rolePrivilege => rolePrivilege.Id);

            foreach (var rolePrivilege in rolePrivileges)
                UnitOfWork.Repository<RolePrivilege>().Delete(rolePrivilege);
        }
        private void CreateRolePrivileges(RoleView view)
        {
            foreach (var privilegeId in view.PrivilegesTree.SelectedIds)
                UnitOfWork.Repository<RolePrivilege>().Insert(new RolePrivilege()
                {
                    RoleId = view.Id,
                    PrivilegeId = privilegeId
                });
        }

        private void RemoveRoleFromUsers(String roleId)
        {
            var usersWithRole = UnitOfWork
                .Repository<User>()
                .Query(user => user.RoleId == roleId);

            foreach (var user in usersWithRole)
            {
                user.RoleId = null;
                UnitOfWork.Repository<User>().Update(user);
            }
        }
    }
}
