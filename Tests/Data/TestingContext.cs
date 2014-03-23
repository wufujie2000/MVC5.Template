using AutoMapper;
using System;
using System.Collections;
using System.Data.Entity;
using Template.Data.Core;
using Template.Data.Mapping;
using Template.Objects;
using Template.Tests.Objects.Components.Services;

namespace Template.Tests.Data
{
    public class TestingContext : Context
    {
        private Hashtable repositories;

        #region DbSets

        #region Account

        private DbSet<Account> Accounts { get; set; }

        #endregion

        #region Administration

        private DbSet<Person> Users { get; set; }
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
            Mapper.CreateMap<TestModel, TestView>();
            Mapper.CreateMap<TestView, TestModel>();
        }
        public TestingContext()
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
