using System;
using System.IO;

namespace Warehouse.Data
{
    public sealed class ExceptionLogger
    {
        private static ExceptionLogger instance;

        private string dir = "Logs";

        public void LogException(string exceptionString, DateTime time)
        {
            string logString = Environment.NewLine + time.ToString("HH:mm") + Environment.NewLine + exceptionString;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (StreamWriter sw = new StreamWriter($"{dir}\\{time.ToString("yyyyMMdd")}.txt", true))
            {
                sw.WriteLineAsync(logString);
            }
        }

        private ExceptionLogger()
        {
        }

        public static ExceptionLogger Instance
        {
            get { return instance ?? (instance = new ExceptionLogger()); }
        }
    }
}
