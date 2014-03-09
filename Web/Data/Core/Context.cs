using System;
using System.Collections;
using System.Data.Entity;
using Template.Data.Mapping;
using Template.Objects;

namespace Template.Data.Core
{
    public class Context : AContext
    {
        private Hashtable repositories;

        #region DbSets

        #region Account

        private DbSet<Account> Accounts { get; set; }

        #endregion

        #region Administration

        private DbSet<User> Users { get; set; }
        private DbSet<Role> Roles { get; set; }
        private DbSet<Privilege> Privileges { get; set; }
        private DbSet<RolePrivilege> RolePrivileges { get; set; }

        #endregion

        #region System

        private DbSet<Language> Languages { get; set; }

        #endregion

        #endregion

        static Context()
        {
            ObjectMapper.MapObjects();
        }
        public Context()
        {
            repositories = new Hashtable();
        }

        public override IRepository<TModel> Repository<TModel>()
        {
            var modelName = typeof(TModel).Name;
            if (repositories.ContainsKey(modelName))
                return (IRepository<TModel>)repositories[modelName];

            var repositoryType = typeof(Repository<>).MakeGenericType(typeof(TModel));
            var modelRepository = (IRepository<TModel>)Activator.CreateInstance(repositoryType, this);
            repositories.Add(modelName, modelRepository);

            return modelRepository;
        }
    }
}
