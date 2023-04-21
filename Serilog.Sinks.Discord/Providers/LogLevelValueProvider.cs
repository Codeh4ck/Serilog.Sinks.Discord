using System.Collections.Generic;
using Discord;
using Serilog.Events;

namespace Serilog.Sinks.Discord.Providers
{
    public class LogLevelValueProvider : IValueProvider<LogSeverity, LogEventLevel>
    {
        private static readonly Dictionary<LogEventLevel, LogSeverity> SerilogToDiscordLevelMap = new Dictionary<LogEventLevel, LogSeverity>();

        static LogLevelProvider()
        {
            SerilogToDiscordLevelMap.Add(LogEventLevel.Verbose, LogSeverity.Verbose);
            SerilogToDiscordLevelMap.Add(LogEventLevel.Information, LogSeverity.Info);
            SerilogToDiscordLevelMap.Add(LogEventLevel.Warning, LogSeverity.Warning);
            SerilogToDiscordLevelMap.Add(LogEventLevel.Error, LogSeverity.Error);
            SerilogToDiscordLevelMap.Add(LogEventLevel.Fatal, LogSeverity.Critical);
            SerilogToDiscordLevelMap.Add(LogEventLevel.Debug, LogSeverity.Debug);
        }
    
        private LogLevelProvider() { }
    
        public LogSeverity Provide(LogEventLevel param) => SerilogToDiscordLevelMap[param];
    }
}