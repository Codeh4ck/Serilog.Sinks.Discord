using Discord;
using Serilog.Events;

namespace Serilog.Sinks.Discord.Providers
{
    public interface IDiscordMessageProvider : IValueProvider<Embed, LogEvent> { }
}