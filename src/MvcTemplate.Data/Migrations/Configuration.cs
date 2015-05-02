using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using System;
using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MvcTemplate.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    internal sealed class Configuration : DbMigrationsConfiguration<Context>, IDisposable
    {
        private IUnitOfWork unitOfWork;

        public Configuration()
        {
            ContextKey = "Context";
        }

        protected override void Seed(Context context)
        {
            IAuditLogger logger = new AuditLogger(new Context(), "sys_seeder");
            unitOfWork = new UnitOfWork(context, logger);

            SeedPrivileges();
            SeedRoles();

            SeedAccounts();
        }

        #region Administration

        private void SeedPrivileges()
        {
            Privilege[] privileges =
            {
                new Privilege { Area = "Administration", Controller = "Accounts", Action = "Index" },
                new Privilege { Area = "Administration", Controller = "Accounts", Action = "Create" },
                new Privilege { Area = "Administration", Controller = "Accounts", Action = "Details" },
                new Privilege { Area = "Administration", Controller = "Accounts", Action = "Edit" },

                new Privilege { Area = "Administration", Controller = "Roles", Action = "Index" },
                new Privilege { Area = "Administration", Controller = "Roles", Action = "Create" },
                new Privilege { Area = "Administration", Controller = "Roles", Action = "Details" },
                new Privilege { Area = "Administration", Controller = "Roles", Action = "Edit" },
                new Privilege { Area = "Administration", Controller = "Roles", Action = "Delete" }
            };

            DeleteUnused(privileges);
            CreateMissing(privileges);
        }
        private void DeleteUnused(Privilege[] privileges)
        {
            foreach (Privilege privilege in unitOfWork.Select<Privilege>())
                if (!privileges.Any(priv => privilege.Area == priv.Area && privilege.Action == priv.Action && privilege.Controller == priv.Controller))
                {
                    foreach (RolePrivilege rolePrivilege in unitOfWork.Select<RolePrivilege>().Where(rolePriv => rolePriv.PrivilegeId == privilege.Id))
                        unitOfWork.Delete(rolePrivilege);

                    unitOfWork.Delete(privilege);
                }

            unitOfWork.Commit();
        }
        private void CreateMissing(Privilege[] privileges)
        {
            Privilege[] dbPrivileges = unitOfWork.Select<Privilege>().ToArray();
            foreach (Privilege privilege in privileges)
                if (!dbPrivileges.Any(priv => privilege.Area == priv.Area && privilege.Action == priv.Action && privilege.Controller == priv.Controller))
                {
                    unitOfWork.Insert(privilege);
                }

            unitOfWork.Commit();
        }

        private void SeedRoles()
        {
            if (!unitOfWork.Select<Role>().Any(role => role.Name == "Sys_Admin"))
            {
                unitOfWork.Insert(new Role { Name = "Sys_Admin" });
                unitOfWork.Commit();
            }

            String adminRoleId = unitOfWork.Select<Role>().Single(role => role.Name == "Sys_Admin").Id;
            RolePrivilege[] adminPrivileges = unitOfWork
                .Select<RolePrivilege>()
                .Where(rolePrivilege => rolePrivilege.RoleId == adminRoleId)
                .ToArray();

            foreach (Privilege privilege in unitOfWork.Select<Privilege>())
                if (!adminPrivileges.Any(rolePrivilege => rolePrivilege.PrivilegeId == privilege.Id))
                    unitOfWork.Insert(new RolePrivilege
                    {
                        RoleId = adminRoleId,
                        PrivilegeId = privilege.Id
                    });

            unitOfWork.Commit();
        }

        private void SeedAccounts()
        {
            Account[] accounts =
            {
                new Account
                {
                    Username = "admin",
                    Passhash = "$2a$13$yTgLCqGqgH.oHmfboFCjyuVUy5SJ2nlyckPFEZRJQrMTZWN.f1Afq", // Admin123?
                    Email = "admin@admins.com",
                    RoleId = unitOfWork.Select<Role>().Single(role => role.Name == "Sys_Admin").Id
                }
            };

            foreach (Account account in accounts)
                if (!unitOfWork.Select<Account>().Any(acc => acc.Username == account.Username))
                    unitOfWork.Insert(account);

            unitOfWork.Commit();
        }

        #endregion

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
