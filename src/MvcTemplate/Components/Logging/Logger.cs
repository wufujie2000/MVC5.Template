using System;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;

namespace MvcTemplate.Components.Logging
{
    public class Logger : ILogger
    {
        private static Object LogWriting = new Object();

        public virtual void Log(String message)
        {
            Log(null, message);
        }
        public virtual void Log(String accountId, String message)
        {
            try
            {
                Int64 backupSize = Int64.Parse(WebConfigurationManager.AppSettings["LogBackupSize"]);
                String logDirectoryPath = WebConfigurationManager.AppSettings["LogsDir"];
                String basePath = HostingEnvironment.ApplicationPhysicalPath ?? "";
                logDirectoryPath = Path.Combine(basePath, logDirectoryPath);
                String logPath = Path.Combine(logDirectoryPath, "Log.txt");

                lock (LogWriting)
                {
                    StringBuilder log = new StringBuilder();
                    log.AppendLine("Time   : " + DateTime.Now);
                    log.AppendLine("Account: " + accountId);
                    log.AppendLine("Message: " + message);
                    log.AppendLine();

                    Directory.CreateDirectory(logDirectoryPath);
                    File.AppendAllText(logPath, log.ToString());

                    if (new FileInfo(logPath).Length >= backupSize)
                    {
                        String backupLog = Path.Combine(logDirectoryPath, String.Format("Log {0}.txt", DateTime.Now.ToString("yyyy-MM-dd HHmmss")));
                        File.Move(logPath, backupLog);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
