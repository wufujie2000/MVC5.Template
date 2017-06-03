using MvcTemplate.Components.Security;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;

namespace MvcTemplate.Components.Logging
{
    public class Logger : ILogger
    {
        private Int32? AccountId { get; }
        private static Object LogWriting = new Object();

        public Logger()
        {
        }
        public Logger(Int32? accountId)
        {
            AccountId = accountId;
        }

        public void Log(String message)
        {
            Int64 backupSize = Int64.Parse(WebConfigurationManager.AppSettings["LogBackupSize"]);
            String logDirectory = WebConfigurationManager.AppSettings["LogDirectory"];
            String basePath = HostingEnvironment.ApplicationPhysicalPath ?? "";
            String logPath = Path.Combine(basePath, logDirectory, "Log.txt");
            Int32? accountId = AccountId ?? HttpContext.Current.User?.Id();

            StringBuilder log = new StringBuilder();
            log.AppendLine("Time   : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            log.AppendLine("Account: " + accountId);
            log.AppendLine("Message: " + message);
            log.AppendLine();

            lock (LogWriting)
            {
                Directory.CreateDirectory(logDirectory);
                File.AppendAllText(logPath, log.ToString());

                if (new FileInfo(logPath).Length >= backupSize)
                    File.Move(logPath, Path.Combine(logDirectory, $"Log {DateTime.Now:yyyy-MM-dd HHmmss}.txt"));
            }
        }
        public void Log(Exception exception)
        {
            while (exception.InnerException != null)
                exception = exception.InnerException;

            String message = $"{exception.GetType()}: {exception.Message}{Environment.NewLine}{exception.StackTrace}";

            Log(message);
        }
    }
}
