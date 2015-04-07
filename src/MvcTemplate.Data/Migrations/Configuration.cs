using MvcTemplate.Data.Core;
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
        private Context context;

        public Configuration()
        {
            ContextKey = "Context";
        }

        protected override void Seed(Context context)
        {
            this.context = context;

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
            foreach (Privilege privilege in context.Set<Privilege>())
                if (!privileges.Any(priv => privilege.Area == priv.Area && privilege.Action == priv.Action && privilege.Controller == priv.Controller))
                {
                    context.Set<RolePrivilege>().RemoveRange(context.Set<RolePrivilege>().Where(rolePriv => rolePriv.PrivilegeId == privilege.Id));
                    context.Set<Privilege>().Remove(privilege);
                }

            context.SaveChanges();
        }
        private void CreateMissing(Privilege[] privileges)
        {
            Privilege[] dbPrivileges = context.Set<Privilege>().ToArray();
            foreach (Privilege privilege in privileges)
                if (!dbPrivileges.Any(priv => privilege.Area == priv.Area && privilege.Action == priv.Action && privilege.Controller == priv.Controller))
                {
                    context.Set<Privilege>().Add(privilege);
                }

            context.SaveChanges();
        }

        private void SeedRoles()
        {
            if (!context.Set<Role>().Any(role => role.Name == "Sys_Admin"))
            {
                context.Set<Role>().Add(new Role { Name = "Sys_Admin" });
                context.SaveChanges();
            }

            String adminRoleId = context.Set<Role>().Single(role => role.Name == "Sys_Admin").Id;
            RolePrivilege[] adminPrivileges = context
                .Set<RolePrivilege>()
                .Where(rolePrivilege => rolePrivilege.RoleId == adminRoleId)
                .ToArray();

            foreach (Privilege privilege in context.Set<Privilege>())
                if (!adminPrivileges.Any(rolePrivilege => rolePrivilege.PrivilegeId == privilege.Id))
                    context.Set<RolePrivilege>().Add(new RolePrivilege
                    {
                        RoleId = adminRoleId,
                        PrivilegeId = privilege.Id
                    });

            context.SaveChanges();
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
                    RoleId = context.Set<Role>().Single(role => role.Name == "Sys_Admin").Id
                }
            };

            foreach (Account account in accounts)
                if (!context.Set<Account>().Any(acc => acc.Username == account.Username))
                    context.Set<Account>().Add(account);

            context.SaveChanges();
        }

        #endregion

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
