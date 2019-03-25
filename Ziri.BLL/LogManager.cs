using System;
using System.IO;
using System.Threading;
using System.Configuration;

namespace Ziri.BLL
{
    public class LogManager
    {
        private string logFilePath;
        private static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();
        public LogManager()
        {
            logFilePath = ConfigurationManager.AppSettings["LOGDIR"] + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            Utils.CreateDirectory(logFilePath);
        }
        public void WriteLog(string logContent)
        {
            try
            {
                LogWriteLock.EnterWriteLock();
                File.AppendAllText(logFilePath, DateTime.Now + ": " + logContent + "\r\n");
            }
            finally { LogWriteLock.ExitWriteLock(); }
        }
    }
}
