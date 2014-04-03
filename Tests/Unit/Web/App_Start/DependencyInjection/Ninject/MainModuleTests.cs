using Ninject;
using NUnit.Framework;
using Template.Components.Security;
using Template.Components.Services;
using Template.Data.Core;
using Template.Data.Logging;
using Template.Web.DependencyInjection.Ninject;

namespace Template.Tests.Unit.Web.DependencyInjection.Ninject
{
    [TestFixture]
    public class MainModuleTests
    {
        private KernelBase kernel;
        private MainModule module;

        [SetUp]
        public void SetUp()
        {
            module = new MainModule();
            kernel = new StandardKernel(module);
        }

        #region Method: Load()

        [Test]
        public void Load_BindsAContext()
        {
            var expected = typeof(Context);
            var actual = kernel.Get<AContext>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIUnitOfWork()
        {
            var expected = typeof(UnitOfWork);
            var actual = kernel.Get<IUnitOfWork>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIEntityLogger()
        {
            var expected = typeof(EntityLogger);
            var actual = kernel.Get<IEntityLogger>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIRoleProvider()
        {
            var expected = typeof(RoleProvider);
            var actual = kernel.Get<IRoleProvider>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIHomeService()
        {
            var expected = typeof(HomeService);
            var actual = kernel.Get<IHomeService>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIRolesService()
        {
            var expected = typeof(RolesService);
            var actual = kernel.Get<IRolesService>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIUsersService()
        {
            var expected = typeof(UsersService);
            var actual = kernel.Get<IUsersService>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIAccountService()
        {
            var expected = typeof(AccountService);
            var actual = kernel.Get<IAccountService>().GetType();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Load_BindsIProfileService()
        {
            var expected = typeof(ProfileService);
            var actual = kernel.Get<IProfileService>().GetType();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
