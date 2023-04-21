using System;
using Discord;
using Serilog.Events;

namespace Serilog.Sinks.Discord.Providers
{
    public interface ILogColorValueProvider : IValueProvider<Color, LogEventLevel>
    {
        public static readonly Lazy<IValueProvider<Color, LogEventLevel>> Default = new Lazy<IValueProvider<Color, LogEventLevel>>(new LogColorValueProvider());
    }
}