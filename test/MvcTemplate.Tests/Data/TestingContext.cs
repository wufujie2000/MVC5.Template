using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data.Mapping;
using MvcTemplate.Tests.Objects;
using System.Data.Entity;

namespace MvcTemplate.Tests.Data
{
    public class TestingContext : Context
    {
        #region Test

        protected DbSet<TestModel> TestModels { get; set; }

        #endregion

        static TestingContext()
        {
            TestObjectMapper.MapObjects();
        }

        public void DropData()
        {
            Set<RolePrivilege>().RemoveRange(Set<RolePrivilege>());
            Set<Privilege>().RemoveRange(Set<Privilege>());
            Set<Account>().RemoveRange(Set<Account>());
            Set<Role>().RemoveRange(Set<Role>());

            SaveChanges();
        }
    }
}
