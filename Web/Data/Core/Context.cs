using MvcTemplate.Data.Mapping;
using MvcTemplate.Objects;
using System;
using System.Collections;
using System.Data.Entity;

namespace MvcTemplate.Data.Core
{
    public class Context : AContext
    {
        protected Hashtable repositories;

        #region DbSets

        #region Account

        protected DbSet<Account> Accounts { get; set; }

        #endregion

        #region Administration

        protected DbSet<Role> Roles { get; set; }
        protected DbSet<Privilege> Privileges { get; set; }
        protected DbSet<RolePrivilege> RolePrivileges { get; set; }

        #endregion

        #region System

        protected DbSet<Log> Logs { get; set; }

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
