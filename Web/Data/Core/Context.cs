using MvcTemplate.Data.Mapping;
using MvcTemplate.Objects;
using System;
using System.Collections;
using System.Data.Entity;

namespace MvcTemplate.Data.Core
{
    public class Context : AContext
    {
        private Hashtable repositories;

        #region DbSets

        #region Account

        private DbSet<Account> Accounts { get; set; }

        #endregion

        #region Administration

        private DbSet<Role> Roles { get; set; }
        private DbSet<Privilege> Privileges { get; set; }
        private DbSet<RolePrivilege> RolePrivileges { get; set; }

        #endregion

        #region System

        private DbSet<Log> Logs { get; set; }
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
            String modelName = typeof(TModel).Name;
            if (repositories.ContainsKey(modelName))
                return (IRepository<TModel>)repositories[modelName];

            Type repositoryType = typeof(Repository<>).MakeGenericType(typeof(TModel));
            IRepository<TModel> modelRepository = (IRepository<TModel>)Activator.CreateInstance(repositoryType, this);
            repositories.Add(modelName, modelRepository);

            return modelRepository;
        }
    }
}
