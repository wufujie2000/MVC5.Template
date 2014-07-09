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
            AutomaticMigrationsEnabled = true;
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
            List<Privilege> privileges = new List<Privilege>();

            privileges.Add(new Privilege() { Area = "Administration", Controller = "Accounts", Action = "Index" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Accounts", Action = "Create" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Accounts", Action = "Details" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Accounts", Action = "Edit" });

            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Index" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Create" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Details" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Edit" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Delete" });

            IEnumerable<Privilege> existingPrivileges = unitOfWork.Repository<Privilege>().Query().ToList();
            foreach (Privilege privilege in privileges)
                if (!existingPrivileges.Any(priv => priv.Area == priv.Area && priv.Controller == priv.Controller && priv.Action == priv.Action))
                    unitOfWork.Repository<Privilege>().Insert(privilege);

            unitOfWork.Commit();
        }
        private void SeedAdministratorRole()
        {
            if (!unitOfWork.Repository<Role>().Query(role => role.Name == "Sys_Admin").Any())
            {
                unitOfWork.Repository<Role>().Insert(new Role() { Name = "Sys_Admin" });
                unitOfWork.Commit();
            }

            String adminRoleId = unitOfWork.Repository<Role>().Query(role => role.Name == "Sys_Admin").First().Id;
            IEnumerable<RolePrivilege> adminPrivileges = unitOfWork
                .Repository<RolePrivilege>()
                .Query(rolePrivilege => rolePrivilege.RoleId == adminRoleId)
                .ToList();

            foreach (Privilege privilege in unitOfWork.Repository<Privilege>().Query())
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
                    RoleId = unitOfWork.Repository<Role>().Query(p => p.Name == "Sys_Admin").First().Id
                },
                new Account()
                {
                    Username = "test",
                    Passhash = "$2a$13$VLUUfSyotu8Ec.D4mZRCE.YuQ5i7CbTi84LGQp1aFb7xvVksPVLdm", // test
                    Email = "test@tests.com"
                }
            };

            foreach (Account account in accounts)
                if (!unitOfWork.Repository<Account>().Query(acc => acc.Username == account.Username).Any())
                    unitOfWork.Repository<Account>().Insert(account);

            unitOfWork.Commit();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
