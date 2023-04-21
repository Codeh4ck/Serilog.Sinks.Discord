using Discord;
using Serilog.Events;
using System.Collections.Generic;

namespace Serilog.Sinks.Discord.Providers
{
    public class LogColorValueProvider : ILogColorValueProvider
    {
        private static readonly Dictionary<LogEventLevel, Color> LogColorMap = new Dictionary<LogEventLevel, Color>();

        static LogColorValueProvider()
        {
            LogColorMap.Add(LogEventLevel.Verbose, Color.Default);
            LogColorMap.Add(LogEventLevel.Information, Color.Blue);
            LogColorMap.Add(LogEventLevel.Warning, Color.Orange);
            LogColorMap.Add(LogEventLevel.Error, Color.Red);
            LogColorMap.Add(LogEventLevel.Fatal, Color.DarkPurple);
            LogColorMap.Add(LogEventLevel.Debug, Color.Green);
        }

        internal LogColorValueProvider() {}
        
        public Color Provide(LogEventLevel logEventLevel) => LogColorMap[logEventLevel];
    }
}