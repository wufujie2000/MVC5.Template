using NUnit.Framework;
using Template.Data.Core;
using Template.Objects;

namespace Template.Tests.Tests.Data.Core
{
    [TestFixture]
    public class ContextTests
    {
        private Context context;

        [SetUp]
        public void SetUp()
        {
            context = new Context();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        #region Method: Repository<TModel>()

        [Test]
        public void Repository_GetsSameRepositoryInstance()
        {
            Assert.AreEqual(context.Repository<Person>(), context.Repository<Person>());
        }

        #endregion
    }
}
