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

            Assert.AreSame(context.Repository<Account>(), context.Repository<Account>());
        }

        #endregion
    }
}
