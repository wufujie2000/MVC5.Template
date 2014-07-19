using MvcTemplate.Data.Core;
using MvcTemplate.Tests.Data.Mapping;
using MvcTemplate.Tests.Objects;
using System.Data.Entity;

namespace MvcTemplate.Tests.Data
{
    public class TestingContext : Context
    {
        #region DbSets

        #region Test

        private DbSet<TestModel> TestModels { get; set; }

        #endregion

        #endregion

        static TestingContext()
        {
            TestObjectMapper.MapObjects();
        }
    }
}
