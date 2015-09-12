using MvcTemplate.Components.Logging;
using System;
using System.IO;
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
                Directory.Delete(logDirectory, true);
        }

        #region Method: Log(String message)

        [Fact]
        public void Log_OnLoggingExceptionDoesNotThrow()
        {
            Directory.CreateDirectory(logDirectory);

            using (FileStream file = File.Create(logPath))
                logger.Log("Test");

            Assert.Empty(File.ReadAllText(logPath));
        }

        [Fact]
        public void Log_LogsMessage()
        {
            logger.Log("Test");

            String expected = "Account: " + Environment.NewLine + "Message: Test" + Environment.NewLine + Environment.NewLine;
            String actual = File.ReadAllText(logPath);

            Assert.True(actual.StartsWith("Time   :"));
            Assert.True(actual.EndsWith(expected));
        }

        [Fact]
        public void Log_OnExceededLogSizeCreatesABackupFile()
        {
            logger.Log(new String('T', backupSize));

            String expected = "Account: " + Environment.NewLine + "Message: " + new String('T', backupSize) + Environment.NewLine + Environment.NewLine;
            String actual = File.ReadAllText(Path.Combine(logDirectory, String.Format("Log {0}.txt", DateTime.Now.ToString("yyyy-MM-dd HHmmss"))));

            Assert.True(actual.StartsWith("Time   :"));
            Assert.True(actual.EndsWith(expected));
        }

        #endregion

        #region Method: Log(String accountId, String message)

        [Fact]
        public void Log_LogsAccountIdAndMessage()
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
