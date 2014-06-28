using MvcTemplate.Data.Core;
using MvcTemplate.Data.Mapping;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data.Mapping;
using MvcTemplate.Tests.Objects;
using System;
using System.Collections;
using System.Data.Entity;

namespace MvcTemplate.Tests.Data
{
    public class TestingContext : Context
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

        #region Test

        private DbSet<TestModel> TestModels { get; set; }

        #endregion

        #endregion

        static TestingContext()
        {
            ObjectMapper.MapObjects();
            TestObjectMapper.MapObjects();
        }
        public TestingContext()
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
