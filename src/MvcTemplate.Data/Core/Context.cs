using MvcTemplate.Data.Mapping;
using MvcTemplate.Objects;
using System;
using System.Collections;
using System.Data.Entity;

namespace MvcTemplate.Data.Core
{
    public class Context : AContext
    {
        protected Hashtable Repositories { get; set; }

        #region DbSets

        #region Administration

        protected DbSet<Account> Accounts { get; set; }

        protected DbSet<Role> Roles { get; set; }
        protected DbSet<Privilege> Privileges { get; set; }
        protected DbSet<RolePrivilege> RolePrivileges { get; set; }

        #endregion

        #region System

        protected DbSet<Log> Logs { get; set; }
        protected DbSet<AuditLog> AuditLogs { get; set; }

        #endregion

        #endregion

        static Context()
        {
            ObjectMapper.MapObjects();
        }
        public Context()
        {
            Repositories = new Hashtable();
        }

        public override IRepository<TModel> Repository<TModel>()
        {
            Type modelType = typeof(TModel);
            if (Repositories.ContainsKey(modelType))
                return (IRepository<TModel>)Repositories[modelType];

            IRepository<TModel> repository = (IRepository<TModel>)Activator.CreateInstance(typeof(Repository<TModel>), this);
            Repositories.Add(modelType, repository);

            return repository;
        }
    }
}
