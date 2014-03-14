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
        private Context context;

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Template.Data";
            context = new Context();
        }

        protected override void Seed(Context context)
        {
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
                if (!context.Repository<Language>().Query().Any(lang => lang.Abbreviation == language.Abbreviation))
                    context.Repository<Language>().Insert(language);

            context.SaveChanges();
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

            var existingPrivileges = context.Repository<Privilege>().Query();
            foreach (var privilege in privileges)
                if (!existingPrivileges.Any(priv => priv.Area == priv.Area && priv.Controller == priv.Controller && priv.Action == priv.Action))
                    context.Repository<Privilege>().Insert(privilege);

            context.SaveChanges();
        }
        private void SeedAdministratorRole()
        {
            if (!context.Repository<Role>().Query(role => role.Name == "Administrator").Any())
                context.Repository<Role>().Insert(new Role() { Name = "Administrator" });

            context.SaveChanges();

            var adminRoleId = context.Repository<Role>().Query(role => role.Name == "Administrator").First().Id;
            var adminPrivileges = context.Repository<RolePrivilege>().Query(rolePrivilege => rolePrivilege.RoleId == adminRoleId);
            foreach (var privilege in context.Repository<Privilege>().Query())
                if (!adminPrivileges.Any(rolePrivilege => rolePrivilege.PrivilegeId == privilege.Id))
                    context.Repository<RolePrivilege>().Insert(new RolePrivilege()
                    {
                        RoleId = adminRoleId,
                        PrivilegeId = privilege.Id
                    });

            context.SaveChanges();
        }
        private void SeedPeople()
        {
            var people = new List<Person>()
            {
                new Person() { FirstName = "System", LastName = "Admin", RoleId = context.Repository<Role>().Query(p => p.Name == "Administrator").First().Id },
                new Person() { FirstName = "Test", LastName = "User", RoleId = context.Repository<Role>().Query(p => p.Name == "Administrator").First().Id },
            };

            foreach (var person in people)
                if (!context.Repository<Person>().Query(u => u.FirstName == person.FirstName && u.LastName == person.LastName).Any())
                    context.Repository<Person>().Insert(person);

            context.SaveChanges();
        }
        private void SeedAccounts()
        {
            var accounts = new List<Account>()
            {
                new Account() { Username = "admin", Passhash = "$2a$13$55S8dVpqNTw2xUBeFNwvIeqM0wf7fkFDHd/tsCYj.9AQooMWep/Yi",
                    PersonId = context.Repository<Person>().Query(p => p.FirstName == "System").First().Id,
                    Id = context.Repository<Person>().Query(p => p.FirstName == "System").First().Id },
                new Account() { Username = "test", Passhash = "$2a$13$VLUUfSyotu8Ec.D4mZRCE.YuQ5i7CbTi84LGQp1aFb7xvVksPVLdm",
                    PersonId = context.Repository<Person>().Query(p => p.FirstName == "Test").First().Id,
                    Id = context.Repository<Person>().Query(p => p.FirstName == "Test").First().Id }
            };

            foreach (var account in accounts)
                if (!context.Repository<Account>().Query(acc => acc.Username == account.Username).Any())
                    context.Repository<Account>().Insert(account);

            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
