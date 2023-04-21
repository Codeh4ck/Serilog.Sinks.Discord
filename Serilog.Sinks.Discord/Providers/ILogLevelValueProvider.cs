using System;
using Discord;
using Serilog.Events;

namespace Serilog.Sinks.Discord.Providers
{
    public interface ILogLevelValueProvider : IValueProvider<LogSeverity, LogEventLevel>
    {
        public static readonly Lazy<IValueProvider<LogSeverity, LogEventLevel>> Default = new Lazy<IValueProvider<LogSeverity, LogEventLevel>>(new LogLevelValueProvider());
    }
}