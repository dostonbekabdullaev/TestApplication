using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
namespace Test.Core.Logger
{
    internal class Logger : ILogger
    {
        private readonly NLog.Logger _logger;
        public Logger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }
        public void LogError(string message, string? loggerName = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0, object[] data = null)
        {
            var logEvent = LogEventInfo.Create(LogLevel.Error, loggerName, CultureInfo.InvariantCulture, message, data);
            logEvent.SetCallerInfo(null, callerMemberName, callerFilePath, callerLineNumber);
            _logger.Log(typeof(Logger), logEvent);
        }

        public void LogInformation(string message, string? loggerName = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0, object[] data = null)
        {
            var logEvent = LogEventInfo.Create(LogLevel.Info, loggerName, CultureInfo.InvariantCulture, message, data);
            logEvent.SetCallerInfo(null, callerMemberName, callerFilePath, callerLineNumber);
            _logger.Log(typeof(Logger), logEvent);
        }
    }
}
