using MvcTemplate.Web;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Reflection;

namespace MvcTemplate.Tests.Unit.Web
{
    [TestFixture]
    public class MvcApplicationTests
    {
        private MvcApplication application;

        [SetUp]
        public void SetUp()
        {
            application = new MvcApplication();
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

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
