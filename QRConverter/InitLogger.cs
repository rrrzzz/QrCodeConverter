using NLog;
using NLog.Config;
using NLog.Targets;

namespace QRConverter
{
    public static class InitLogger
    {
        public static Logger GetLogger(string name)
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            
            consoleTarget.Layout = @"${message}";
            fileTarget.Layout = @"${longdate} | ${level: uppercase = true} | ${message} | ${exception: format = tostring}";
            fileTarget.FileName = "${basedir}/errorLog.txt"; 

            var rule1 = new LoggingRule("*", LogLevel.Info, LogLevel.Info, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Error, fileTarget);
            config.LoggingRules.Add(rule2);

            LogManager.Configuration = config;

            return LogManager.GetLogger(name);
        }
    }
}
