using MvcTemplate.Components.Logging;
using System;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Logging
{
    public class LoggerTests
    {
        private String logDirectory;
        private Int32 backupSize;
        private String logPath;
        private Logger logger;

        public LoggerTests()
        {
            backupSize = Int32.Parse(WebConfigurationManager.AppSettings["LogBackupSize"]);
            logDirectory = WebConfigurationManager.AppSettings["LogsDir"];
            logPath = Path.Combine(logDirectory, "Log.txt");
            logger = new Logger();

            if (Directory.Exists(logDirectory))
            {
                String[] files = Directory.GetFiles(logDirectory);
                foreach (String file in files) File.Delete(file);

                Directory.Delete(logDirectory);
            }
        }

        #region Method: Log(String message)

        [Fact]
        public void Log_DoesNotThrow()
        {
            Directory.CreateDirectory(logDirectory);

            using (File.Create(logPath))
                logger.Log("Test");

            Assert.Empty(File.ReadAllText(logPath));
        }

        [Fact]
        public void Log_Message()
        {
            logger.Log("Test");

            String expected = "Account: " + Environment.NewLine + "Message: Test" + Environment.NewLine + Environment.NewLine;
            String actual = File.ReadAllText(logPath);

            Assert.True(actual.StartsWith("Time   :"));
            Assert.True(actual.EndsWith(expected));
        }

        [Fact]
        public void Log_CreatesBackupFile()
        {
            logger.Log(new String('T', backupSize));

            String expected = "Account: " + Environment.NewLine + "Message: " + new String('T', backupSize) + Environment.NewLine + Environment.NewLine;
            String actual = File.ReadAllText(Directory.GetFiles(logDirectory, "Log *.txt").Single());

            Assert.True(actual.StartsWith("Time   :"));
            Assert.True(actual.EndsWith(expected));
        }

        #endregion

        #region Method: Log(String accountId, String message)

        [Fact]
        public void Log_AccountIdAndMessage()
        {
            logger.Log("AccountTest", "MessageTest");

            String expected = "Account: AccountTest" + Environment.NewLine + "Message: MessageTest" + Environment.NewLine + Environment.NewLine;
            String actual = File.ReadAllText(logPath);

            Assert.True(actual.StartsWith("Time   :"));
            Assert.True(actual.EndsWith(expected));
        }

        #endregion
    }
}
