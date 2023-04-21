using System;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.Discord.Models;

namespace Serilog.Sinks.Discord
{
    public class DiscordSink : ILogEventSink
    {
        private readonly DiscordClient _discordClient;
        private readonly SinkConfiguration _sinkConfiguration;

        public DiscordSink(DiscordClient discordClient, SinkConfiguration sinkConfiguration)
        {
            _discordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            _sinkConfiguration = sinkConfiguration ?? throw new ArgumentNullException(nameof(sinkConfiguration));
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent.Level < _sinkConfiguration.LogLevel) return;
            _discordClient.PostWebhook(logEvent);
        }
    }
}