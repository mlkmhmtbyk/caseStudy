using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace caseStudy.Services
{
    public class Logger
    {
        private readonly string _logFilePath;

        public Logger()
        {
            _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs.txt");
        }

        public void Log(string message)
        {
            string logMessage = $"{DateTime.Now} - {message}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, logMessage);
        }
        public void Log(string message, string details)
        {
            string logMessage = $"{DateTime.Now} - {message} - {details}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, logMessage);
        }

        public void Log(Exception ex)
        {
            string logMessage = $"{DateTime.Now} - {ex.Message}{Environment.NewLine}";
            logMessage += $"Error: {ex.Message}{Environment.NewLine}";
            logMessage += $"File: {ex.Source}{Environment.NewLine}";
            logMessage += $"Line: {ex.StackTrace}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, logMessage);
        }

        public void Log(string message, Exception ex)
        {
            string logMessage = $"{DateTime.Now} - {message}{Environment.NewLine}";
            logMessage += $"Error: {ex.Message}{Environment.NewLine}";
            logMessage += $"File: {ex.Source}{Environment.NewLine}";
            logMessage += $"Line: {ex.StackTrace}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, logMessage);
        }
        public void LogError(Exception ex)
        {
            string logMessage = $"{DateTime.Now} - {ex.Message}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, logMessage);
        }
        public void LogError(Exception ex, string details)
        {
            string logMessage = $"{DateTime.Now} - {ex.Message}{Environment.NewLine}";
            logMessage += $"{details}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, logMessage);            
        }
        
    }
}