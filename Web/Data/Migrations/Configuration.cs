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
            SeedPeople();
            SeedAccounts();
        }
        private void SeedLanguages()
        {
            var languages = new List<Language>()
            {
                new Language() { Abbreviation = "en-GB", Name = "English" },
                new Language() { Abbreviation = "lt-LT", Name = "Lietuvių" }
            };

            foreach (var language in languages)
                if (!unitOfWork.Repository<Language>().Query().Any(lang => lang.Abbreviation == language.Abbreviation))
                    unitOfWork.Repository<Language>().Insert(language);

            unitOfWork.Commit();
        }
        private void SeedAllPrivileges()
        {
            var privileges = new List<Privilege>();
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Users", Action = "Index" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Users", Action = "Create" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Users", Action = "Details" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Users", Action = "Edit" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Users", Action = "Delete" });

            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Index" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Create" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Details" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Edit" });
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Delete" });

            var existingPrivileges = unitOfWork.Repository<Privilege>().Query();
            foreach (var privilege in privileges)
                if (!existingPrivileges.Any(priv => priv.Area == priv.Area && priv.Controller == priv.Controller && priv.Action == priv.Action))
                    unitOfWork.Repository<Privilege>().Insert(privilege);

            unitOfWork.Commit();
        }
        private void SeedAdministratorRole()
        {
            if (!unitOfWork.Repository<Role>().Query(role => role.Name == "Administrator").Any())
                unitOfWork.Repository<Role>().Insert(new Role() { Name = "Administrator" });

            unitOfWork.Commit();

            var adminRoleId = unitOfWork.Repository<Role>().Query(role => role.Name == "Administrator").First().Id;
            var adminPrivileges = unitOfWork.Repository<RolePrivilege>().Query(rolePrivilege => rolePrivilege.RoleId == adminRoleId);
            foreach (var privilege in unitOfWork.Repository<Privilege>().Query())
                if (!adminPrivileges.Any(rolePrivilege => rolePrivilege.PrivilegeId == privilege.Id))
                    unitOfWork.Repository<RolePrivilege>().Insert(new RolePrivilege()
                    {
                        RoleId = adminRoleId,
                        PrivilegeId = privilege.Id
                    });

            unitOfWork.Commit();
        }
        private void SeedPeople()
        {
            var people = new List<Person>()
            {
                new Person() { FirstName = "System", LastName = "Admin", RoleId = unitOfWork.Repository<Role>().Query(p => p.Name == "Administrator").First().Id },
                new Person() { FirstName = "Test", LastName = "User", RoleId = unitOfWork.Repository<Role>().Query(p => p.Name == "Administrator").First().Id },
            };

            foreach (var person in people)
                if (!unitOfWork.Repository<Person>().Query(u => u.FirstName == person.FirstName && u.LastName == person.LastName).Any())
                    unitOfWork.Repository<Person>().Insert(person);

            unitOfWork.Commit();
        }
        private void SeedAccounts()
        {
            var accounts = new List<Account>()
            {
                new Account() { Username = "admin", Passhash = "$2a$13$yTgLCqGqgH.oHmfboFCjyuVUy5SJ2nlyckPFEZRJQrMTZWN.f1Afq",
                    PersonId = unitOfWork.Repository<Person>().Query(p => p.FirstName == "System").First().Id,
                    Id = unitOfWork.Repository<Person>().Query(p => p.FirstName == "System").First().Id },
                new Account() { Username = "test", Passhash = "$2a$13$VLUUfSyotu8Ec.D4mZRCE.YuQ5i7CbTi84LGQp1aFb7xvVksPVLdm",
                    PersonId = unitOfWork.Repository<Person>().Query(p => p.FirstName == "Test").First().Id,
                    Id = unitOfWork.Repository<Person>().Query(p => p.FirstName == "Test").First().Id }
            };

            foreach (var account in accounts)
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
