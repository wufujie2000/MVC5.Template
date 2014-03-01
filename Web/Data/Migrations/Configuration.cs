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
            context = new Context();
            AutomaticMigrationsEnabled = true;
            ContextKey = "Template.Data";
        }

        protected override void Seed(Context context)
        {
            SeedLanguages();
            SeedAllPrivileges();
            SeedAdministratorRole();
            SeedUsers();
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
                if (!context.Repository<Language>().Query().Any(lang => lang.Abbreviation == "en-GB"))
                    context.Repository<Language>().Insert(language);

            context.SaveChanges();
        }
        private void SeedAllPrivileges()
        {
            var langEN = context.Repository<Language>().Query(language => language.Abbreviation == "en-GB").First().Id;
            var langLT = context.Repository<Language>().Query(language => language.Abbreviation == "lt-LT").First().Id;

            var privileges = new List<Privilege>();
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Users", Action = "Index" });
            privileges.Last().PrivilegeLanguages = new List<PrivilegeLanguage>()
            {
                new PrivilegeLanguage() { Area = "Administration", Controller = "Users", Action = "Index", PrivilegeId = privileges.Last().Id, LanguageId = langEN },
                new PrivilegeLanguage() { Area = "Administravimas", Controller = "Vartotojai", Action = "Peržiūrėti", PrivilegeId = privileges.Last().Id, LanguageId = langLT }
            };
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Users", Action = "Create" });
            privileges.Last().PrivilegeLanguages = new List<PrivilegeLanguage>()
            {
                new PrivilegeLanguage() { Area = "Administration", Controller = "Users", Action = "Create", PrivilegeId = privileges.Last().Id, LanguageId = langEN },
                new PrivilegeLanguage() { Area = "Administravimas", Controller = "Vartotojai", Action = "Kurti", PrivilegeId = privileges.Last().Id, LanguageId = langLT }
            };
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Users", Action = "Details" });
            privileges.Last().PrivilegeLanguages = new List<PrivilegeLanguage>()
            {
                new PrivilegeLanguage() { Area = "Administration", Controller = "Users", Action = "Details", PrivilegeId = privileges.Last().Id, LanguageId = langEN },
                new PrivilegeLanguage() { Area = "Administravimas", Controller = "Vartotojai", Action = "Peržiūrėti detaliai", PrivilegeId = privileges.Last().Id, LanguageId = langLT }
            };
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Users", Action = "Edit" });
            privileges.Last().PrivilegeLanguages = new List<PrivilegeLanguage>()
            {
                new PrivilegeLanguage() { Area = "Administration", Controller = "Users", Action = "Edit", PrivilegeId = privileges.Last().Id, LanguageId = langEN },
                new PrivilegeLanguage() { Area = "Administravimas", Controller = "Vartotojai", Action = "Redaguoti", PrivilegeId = privileges.Last().Id, LanguageId = langLT }
            };
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Users", Action = "Delete" });
            privileges.Last().PrivilegeLanguages = new List<PrivilegeLanguage>()
            {
                new PrivilegeLanguage() { Area = "Administration", Controller = "Users", Action = "Delete", PrivilegeId = privileges.Last().Id, LanguageId = langEN },
                new PrivilegeLanguage() { Area = "Administravimas", Controller = "Vartotojai", Action = "Trinti", PrivilegeId = privileges.Last().Id, LanguageId = langLT }
            };

            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Index" });
            privileges.Last().PrivilegeLanguages = new List<PrivilegeLanguage>()
            {
                new PrivilegeLanguage() { Area = "Administration", Controller = "Roles", Action = "Index", PrivilegeId = privileges.Last().Id, LanguageId = langEN },
                new PrivilegeLanguage() { Area = "Administravimas", Controller = "Rolės", Action = "Peržiūrėti", PrivilegeId = privileges.Last().Id, LanguageId = langLT }
            };
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Create" });
            privileges.Last().PrivilegeLanguages = new List<PrivilegeLanguage>()
            {
                new PrivilegeLanguage() { Area = "Administration", Controller = "Roles", Action = "Create", PrivilegeId = privileges.Last().Id, LanguageId = langEN },
                new PrivilegeLanguage() { Area = "Administravimas", Controller = "Rolės", Action = "Kurti", PrivilegeId = privileges.Last().Id, LanguageId = langLT }
            };
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Details" });
            privileges.Last().PrivilegeLanguages = new List<PrivilegeLanguage>()
            {
                new PrivilegeLanguage() { Area = "Administration", Controller = "Roles", Action = "Details", PrivilegeId = privileges.Last().Id, LanguageId = langEN },
                new PrivilegeLanguage() { Area = "Administravimas", Controller = "Rolės", Action = "Peržiūrėti detaliai", PrivilegeId = privileges.Last().Id, LanguageId = langLT }
            };
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Edit" });
            privileges.Last().PrivilegeLanguages = new List<PrivilegeLanguage>()
            {
                new PrivilegeLanguage() { Area = "Administration", Controller = "Roles", Action = "Edit", PrivilegeId = privileges.Last().Id, LanguageId = langEN },
                new PrivilegeLanguage() { Area = "Administravimas", Controller = "Rolės", Action = "Redaguoti", PrivilegeId = privileges.Last().Id, LanguageId = langLT }
            };
            privileges.Add(new Privilege() { Area = "Administration", Controller = "Roles", Action = "Delete" });
            privileges.Last().PrivilegeLanguages = new List<PrivilegeLanguage>()
            {
                new PrivilegeLanguage() { Area = "Administration", Controller = "Roles", Action = "Delete", PrivilegeId = privileges.Last().Id, LanguageId = langEN },
                new PrivilegeLanguage() { Area = "Administravimas", Controller = "Rolės", Action = "Trinti", PrivilegeId = privileges.Last().Id, LanguageId = langLT }
            };

            var existingPrivileges = context.Repository<Privilege>().Query();
            foreach (var privilege in privileges)
                if (!existingPrivileges.Any(priv => priv.Area == priv.Area && priv.Controller == priv.Controller && priv.Action == priv.Action))
                {
                    context.Repository<Privilege>().Insert(privilege);
                    foreach (var privilegeLanguage in privilege.PrivilegeLanguages)
                        context.Repository<PrivilegeLanguage>().Insert(privilegeLanguage);
                }

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
        private void SeedUsers()
        {
            var users = new List<User>()
            {
                new User() { FirstName = "System", LastName = "Admin", RoleId = context.Repository<Role>().Query(p => p.Name == "Administrator").First().Id },
                new User() { FirstName = "Test", LastName = "User", RoleId = context.Repository<Role>().Query(p => p.Name == "Administrator").First().Id },
            };

            foreach (var user in users)
                if (!context.Repository<User>().Query(u => u.FirstName == user.FirstName && u.LastName == user.LastName).Any())
                    context.Repository<User>().Insert(user);

            context.SaveChanges();
        }
        private void SeedAccounts()
        {
            var accounts = new List<Account>()
            {
                new Account() { Username = "admin", Passhash = "$2a$13$55S8dVpqNTw2xUBeFNwvIeqM0wf7fkFDHd/tsCYj.9AQooMWep/Yi",
                    UserId = context.Repository<User>().Query(p => p.FirstName == "System").First().Id,
                    Id = context.Repository<User>().Query(p => p.FirstName == "System").First().Id },
                new Account() { Username = "test", Passhash = "$2a$13$VLUUfSyotu8Ec.D4mZRCE.YuQ5i7CbTi84LGQp1aFb7xvVksPVLdm",
                    UserId = context.Repository<User>().Query(p => p.FirstName == "Test").First().Id,
                    Id = context.Repository<User>().Query(p => p.FirstName == "Test").First().Id }
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
