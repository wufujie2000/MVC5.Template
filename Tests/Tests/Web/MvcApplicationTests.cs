using NUnit.Framework;
using System.Diagnostics;
using System.Reflection;
using Template.Web;

namespace Template.Tests.Tests.Web
{
    [TestFixture]
    public class MvcApplicationTests
    {
        private MvcApplication application;

        [SetUp]
        public void SetUp()
        {
            application = new MvcApplication();
            // TODO: Add tests for application start?
        }

        [TearDown]
        public void TearDown()
        {
            application.Dispose();
        }

        #region Static property: Version

        [Test]
        public void Version_ReturnsCurrentVersion()
        {
            var assembly = Assembly.GetAssembly(typeof(MvcApplication));
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            
            var expected = versionInfo.FileVersion;
            var actual = MvcApplication.Version;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
