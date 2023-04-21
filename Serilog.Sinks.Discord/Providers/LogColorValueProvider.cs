using Discord;
using Serilog.Events;

namespace Serilog.Sinks.Discord.Providers
{
    public class LogColorValueProvider : IValueProvider<Color, LogEventLevel>
    {
        public Color Provide(LogEventLevel param)
        {
            throw new System.NotImplementedException();
        }
    }
}