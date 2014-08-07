using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using NUnit.Framework;

namespace MvcTemplate.Tests.Unit.Data.Core
{
    [TestFixture]
    public class ContextTests
    {
        #region Method: Repository<TModel>()

        [Test]
        public void Repository_GetsSameRepositoryInstance()
        {
            Context context = new Context();

            IRepository<Account> expected = context.Repository<Account>();
            IRepository<Account> actual = context.Repository<Account>();

            Assert.AreSame(expected, actual);
        }

        #endregion
    }
}
