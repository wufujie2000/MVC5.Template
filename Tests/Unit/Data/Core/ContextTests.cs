using NUnit.Framework;
using Template.Data.Core;
using Template.Objects;

namespace Template.Tests.Unit.Data.Core
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
