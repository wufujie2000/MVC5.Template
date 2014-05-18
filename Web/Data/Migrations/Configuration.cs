using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Template.Data.Core;
using Template.Objects;

namespace Template.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    internal sealed class Configuration : DbMigrationsConfiguration<Context>, IDisposable
    {
        private UnitOfWork unitOfWork;

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Template.Data";
        }

        protected override void Seed(Context context)
        {
            unitOfWork = new UnitOfWork(context);

            SeedLanguages();
            SeedAllPrivileges();
            SeedAdministratorRole();
            SeedAccounts();
        }
        private void SeedLanguages()
        {
            List<Language> languages = new List<Language>()
            {
                new Language() { Abbreviation = "en-GB", Name = "English" },
                new Language() { Abbreviation = "lt-LT", Name = "Lietuvių" }
            };

            foreach (Language language in languages)
                if (!unitOfWork.Repository<Language>().Query().Any(lang => lang.Abbreviation == language.Abbreviation))
                    unitOfWork.Repository<Language>().Insert(language);

            unitOfWork.Commit();
        }
        private void SeedAllPrivileges()
        {
            List<Privilege> privileges = new List<Privilege>();

            privileges.Add(new Privilege() { Area = "Administration", Controller = "Akkounts", Action = "Index" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Akkounts", Action = "Create" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Akkounts", Action = "Details" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Akkounts", Action = "Edit" });

            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Index" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Create" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Details" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Edit" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Delete" });

            IQueryable<Privilege> existingPrivileges = unitOfWork.Repository<Privilege>().Query();
            foreach (Privilege privilege in privileges)
                if (!existingPrivileges.Any(priv => priv.Area == priv.Area && priv.Controller == priv.Controller && priv.Action == priv.Action))
                    unitOfWork.Repository<Privilege>().Insert(privilege);

            unitOfWork.Commit();
        }
        private void SeedAdministratorRole()
        {
            if (!unitOfWork.Repository<Role>().Query(role => role.Name == "Sys_Admin").Any())
                unitOfWork.Repository<Role>().Insert(new Role() { Name = "Sys_Admin" });

            unitOfWork.Commit();

            String adminRoleId = unitOfWork.Repository<Role>().Query(role => role.Name == "Sys_Admin").First().Id;
            IQueryable<RolePrivilege> adminPrivileges = unitOfWork.Repository<RolePrivilege>().Query(rolePrivilege => rolePrivilege.RoleId == adminRoleId);
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
                    RoleId = unitOfWork.Repository<Role>().Query(p => p.Name == "Sys_Admin").First().Id
                },
                new Account()
                {
                    Username = "test",
                    Passhash = "$2a$13$VLUUfSyotu8Ec.D4mZRCE.YuQ5i7CbTi84LGQp1aFb7xvVksPVLdm" // test
                }
            };

            foreach (Account account in accounts)
                if (!unitOfWork.Repository<Account>().Query(acc => acc.Username == account.Username).Any())
                    unitOfWork.Repository<Account>().Insert(account);

            unitOfWork.Commit();

            List<Akkount> akkounts = new List<Akkount>()
            {
                new Akkount()
                {
                    Username = "admin",
                    Passhash = "$2a$13$yTgLCqGqgH.oHmfboFCjyuVUy5SJ2nlyckPFEZRJQrMTZWN.f1Afq", // Admin123?
                    RoleId = unitOfWork.Repository<Role>().Query(p => p.Name == "Sys_Admin").First().Id
                },
                new Akkount()
                {
                    Username = "test",
                    Passhash = "$2a$13$VLUUfSyotu8Ec.D4mZRCE.YuQ5i7CbTi84LGQp1aFb7xvVksPVLdm" // test
                }
            };

            foreach (Akkount account in akkounts)
                if (!unitOfWork.Repository<Akkount>().Query(acc => acc.Username == account.Username).Any())
                    unitOfWork.Repository<Akkount>().Insert(account);

            unitOfWork.Commit();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
