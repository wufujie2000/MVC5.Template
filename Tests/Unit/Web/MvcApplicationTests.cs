using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Reflection;
using Template.Web;

namespace Template.Tests.Unit.Web
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
            Assembly assembly = Assembly.GetAssembly(typeof(MvcApplication));
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            
            String expected = versionInfo.FileVersion;
            String actual = MvcApplication.Version;
            // TODO: Add git hook for increasing version
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
