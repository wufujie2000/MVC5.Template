using MvcTemplate.Components.Logging;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Logging
{
    public class LoggerTests : IDisposable
    {
        private String logDirectory;
        private Int32 backupSize;
        private String logPath;

        public LoggerTests()
        {
            backupSize = Int32.Parse(WebConfigurationManager.AppSettings["LogBackupSize"]);
            logDirectory = WebConfigurationManager.AppSettings["LogsDir"];
            logPath = Path.Combine(logDirectory, "Log.txt");

            if (Directory.Exists(logDirectory))
            {
                String[] files = Directory.GetFiles(logDirectory);
                foreach (String file in files) File.Delete(file);

                Directory.Delete(logDirectory);
            }
        }
        public void Dispose()
        {
            HttpContext.Current = null;
        }

        #region Method: Log(String message)

        [Fact]
        public void Log_MessageForCurrentAccount()
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            Logger logger = new Logger();

            logger.Log("Test");

            String expected = "Account: 1" + Environment.NewLine + "Message: Test" + Environment.NewLine + Environment.NewLine;
            String actual = File.ReadAllText(logPath);

            Assert.True(actual.StartsWith("Time   :"));
            Assert.True(actual.EndsWith(expected));
        }

        [Fact]
        public void Log_CreatesBackupFile()
        {
            Logger logger = new Logger(2);

            logger.Log(new String('T', backupSize));

            String expected = "Account: 2" + Environment.NewLine + "Message: " + new String('T', backupSize) + Environment.NewLine + Environment.NewLine;
            String actual = File.ReadAllText(Directory.GetFiles(logDirectory, "Log *.txt").Single());

            Assert.True(actual.StartsWith("Time   :"));
            Assert.True(actual.EndsWith(expected));
        }

        #endregion
    }
}
