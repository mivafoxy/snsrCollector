using Serilog;
using snsrCollector.core;
using snsrCollector.utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace snsrCollector.logUtils
{
    // Обертка над фреймворком логирования.
    public class SnsrLogger
    {
        const string debugLevel = "DEBUG";
        const string warnLevel = "WARNING";
        const string infoLevel = "INFO";

        public SnsrLogger()
        {
            InitializeLogger();
        }

        public void LogInfo(string message)
        {
            Log.Information(message);
        }

        public void LogWarn(string message)
        {
            Log.Warning(message);
        }

        public void LogDebug(string message)
        {
            Log.Debug(message);
        }

        public void LogError(string message)
        {
            Log.Error(message);
        }

        private void InitializeLogger()
        {
            LoggerConfiguration loggerConfiguration = NewLoggerConfiguration();
            Log.Logger = loggerConfiguration.CreateLogger();
        }

        private LoggerConfiguration NewLoggerConfiguration()
        {
            var loggerConfig = new LoggerConfiguration();

            string loggerLevel = ApplicationCore.GetInstance().GetAppConfig().GetKeyValueOr("logLevel", infoLevel);

            if (loggerLevel == debugLevel)
                loggerConfig.MinimumLevel.Debug();
            else if (loggerLevel == warnLevel)
                loggerConfig.MinimumLevel.Warning();
            else if (loggerLevel == infoLevel)
                loggerConfig.MinimumLevel.Information();

            string logFile = ApplicationCore.GetInstance().GetAppConfig().GetKeyValueOr("logPath", "snsrLog.log");

            loggerConfig.WriteTo.File(logFile, rollingInterval: RollingInterval.Day);
            loggerConfig.WriteTo.Console();

            return loggerConfig;
        }
    }
}
