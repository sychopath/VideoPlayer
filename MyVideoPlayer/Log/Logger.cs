using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVideoPlayer.Log
{
    /// <summary>
    /// The Logger class provides methods for logging messages throughout application.
    /// </summary>
    public static class Logger
    {
        private static string logFilePath;

        public static void Initialize(string filePath)
        {
            logFilePath = filePath;
        }

        // Log a message to the file
        public static void Log(string message)
        {
            File.AppendAllText(logFilePath, $"{DateTime.Now} - {message}\n");
        }
    }
}
