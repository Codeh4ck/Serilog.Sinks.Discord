using Discord;
using Serilog.Events;
using System.Collections.Generic;

namespace Serilog.Sinks.Discord.Providers
{
    public class LogLevelValueProvider : ILogLevelValueProvider
    {
        private static readonly Dictionary<LogEventLevel, LogSeverity> SerilogToDiscordLevelMap = new Dictionary<LogEventLevel, LogSeverity>();

        static LogLevelValueProvider()
        {
            SerilogToDiscordLevelMap.Add(LogEventLevel.Verbose, LogSeverity.Verbose);
            SerilogToDiscordLevelMap.Add(LogEventLevel.Information, LogSeverity.Info);
            SerilogToDiscordLevelMap.Add(LogEventLevel.Warning, LogSeverity.Warning);
            SerilogToDiscordLevelMap.Add(LogEventLevel.Error, LogSeverity.Error);
            SerilogToDiscordLevelMap.Add(LogEventLevel.Fatal, LogSeverity.Critical);
            SerilogToDiscordLevelMap.Add(LogEventLevel.Debug, LogSeverity.Debug);
        }
    
        internal LogLevelValueProvider() { }
    
        public LogSeverity Provide(LogEventLevel param) => SerilogToDiscordLevelMap[param];
    }
}