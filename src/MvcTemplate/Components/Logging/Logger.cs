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

        public void Log(String message)
        {
            Log(null, message);
        }
        public void Log(Int32? accountId, String message)
        {
            try
            {
                Int64 backupSize = Int64.Parse(WebConfigurationManager.AppSettings["LogBackupSize"]);
                String logDirectoryPath = WebConfigurationManager.AppSettings["LogsDir"];
                String basePath = HostingEnvironment.ApplicationPhysicalPath ?? "";
                logDirectoryPath = Path.Combine(basePath, logDirectoryPath);
                String logPath = Path.Combine(logDirectoryPath, "Log.txt");

                StringBuilder log = new StringBuilder();
                log.AppendLine("Time   : " + DateTime.Now);
                log.AppendLine("Account: " + accountId);
                log.AppendLine("Message: " + message);
                log.AppendLine();

                lock (LogWriting)
                {
                    Directory.CreateDirectory(logDirectoryPath);
                    File.AppendAllText(logPath, log.ToString());

                    if (new FileInfo(logPath).Length >= backupSize)
                    {
                        String logBackupFile = String.Format("Log {0}.txt", DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                        String backupPath = Path.Combine(logDirectoryPath, logBackupFile);
                        File.Move(logPath, backupPath);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
