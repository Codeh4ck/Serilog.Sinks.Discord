using System;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Discord.Models;

namespace Serilog.Sinks.Discord
{
    /// <summary>
    /// A Serilog sink that writes events to a Discord channel through webhooks.
    /// </summary>
    public class DiscordSink : ILogEventSink
    {
        private readonly IDiscordClient _discordClient;
        private readonly SinkConfiguration _sinkConfiguration;

        public DiscordSink(IDiscordClient discordClient, SinkConfiguration sinkConfiguration)
        {
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            _sinkConfiguration = sinkConfiguration ?? throw new ArgumentNullException(nameof(sinkConfiguration));
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent.Level < _sinkConfiguration.LogLevel) return;
            _discordClient.ProcessLogEvent(logEvent);
        }
    }
}