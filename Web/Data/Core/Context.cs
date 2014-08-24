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
            String name = typeof(TModel).Name;
            if (repositories.ContainsKey(name))
                return (IRepository<TModel>)repositories[name];

            Type repositoryType = typeof(Repository<>).MakeGenericType(typeof(TModel));
            IRepository<TModel> repository = (IRepository<TModel>)Activator.CreateInstance(repositoryType, this);
            repositories.Add(name, repository);

            return repository;
        }
    }
}
