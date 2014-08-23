using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MvcTemplate.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    internal sealed class Configuration : DbMigrationsConfiguration<Context>, IDisposable
    {
        private UnitOfWork unitOfWork;

        public Configuration()
        {
            ContextKey = "MvcTemplate.Data";
        }

        protected override void Seed(Context context)
        {
            unitOfWork = new UnitOfWork(context);

            SeedAllPrivileges();
            SeedAdministratorRole();
            SeedAccounts();
        }
        private void SeedAllPrivileges()
        {
            List<Privilege> allPrivileges = new List<Privilege>()
            {
                new Privilege() { Area = "Administration", Controller = "Accounts", Action = "Index" },
                new Privilege() { Area = "Administration", Controller = "Accounts", Action = "Create" },
                new Privilege() { Area = "Administration", Controller = "Accounts", Action = "Details" },
                new Privilege() { Area = "Administration", Controller = "Accounts", Action = "Edit" },

                new Privilege() { Area = "Administration", Controller = "Roles", Action = "Index" },
                new Privilege() { Area = "Administration", Controller = "Roles", Action = "Create" },
                new Privilege() { Area = "Administration", Controller = "Roles", Action = "Details" },
                new Privilege() { Area = "Administration", Controller = "Roles", Action = "Edit" },
                new Privilege() { Area = "Administration", Controller = "Roles", Action = "Delete" }
            };

            IEnumerable<Privilege> privileges = unitOfWork.Repository<Privilege>().ToList();
            foreach (Privilege privilege in allPrivileges)
                if (!privileges.Any(priv =>
                        priv.Area == priv.Area &&
                        priv.Action == priv.Action &&
                        priv.Controller == priv.Controller))
                    unitOfWork.Repository<Privilege>().Insert(privilege);

            unitOfWork.Commit();
        }
        private void SeedAdministratorRole()
        {
            if (!unitOfWork.Repository<Role>().Any(role => role.Name == "Sys_Admin"))
            {
                unitOfWork.Repository<Role>().Insert(new Role() { Name = "Sys_Admin" });
                unitOfWork.Commit();
            }

            String adminRoleId = unitOfWork.Repository<Role>().First(role => role.Name == "Sys_Admin").Id;
            IEnumerable<RolePrivilege> adminPrivileges = unitOfWork
                .Repository<RolePrivilege>()
                .Where(rolePrivilege => rolePrivilege.RoleId == adminRoleId)
                .ToList();

            foreach (Privilege privilege in unitOfWork.Repository<Privilege>())
                if (!adminPrivileges.Any(rolePrivilege => rolePrivilege.PrivilegeId == privilege.Id))
                    unitOfWork.Repository<RolePrivilege>().Insert(new RolePrivilege()
                    {
                        RoleId = adminRoleId,
                        PrivilegeId = privilege.Id
                    });

            unitOfWork.Commit();
        }
        private void SeedAccounts()
        {
            List<Account> accounts = new List<Account>()
            {
                new Account()
                {
                    Username = "admin",
                    Passhash = "$2a$13$yTgLCqGqgH.oHmfboFCjyuVUy5SJ2nlyckPFEZRJQrMTZWN.f1Afq", // Admin123?
                    Email = "admin@admins.com",
                    RoleId = unitOfWork.Repository<Role>().First(p => p.Name == "Sys_Admin").Id
                },
                new Account()
                {
                    Username = "test",
                    Passhash = "$2a$13$VLUUfSyotu8Ec.D4mZRCE.YuQ5i7CbTi84LGQp1aFb7xvVksPVLdm", // test
                    Email = "test@tests.com"
                }
            };

            foreach (Account account in accounts)
                if (!unitOfWork.Repository<Account>().Any(acc => acc.Username == account.Username))
                    unitOfWork.Repository<Account>().Insert(account);

            unitOfWork.Commit();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
