using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord;
using Discord.Rest;
using Discord.Webhook;
using Serilog.Events;
using Serilog.Sinks.Discord.Models;
using Serilog.Sinks.Discord.Providers;

namespace Serilog.Sinks.Discord
{
    public class DiscordClient : IDisposable
    {
        private readonly SinkConfiguration _sinkConfiguration;
        private readonly DiscordWebhookClient _discordWebhookClient;
        private readonly IValueProvider<Embed, LogEvent> _discordMessageProvider;

        internal DiscordClient(SinkConfiguration sinkConfiguration,
            IValueProvider<Embed, LogEvent> discordMessageProvider,
            IValueProvider<LogSeverity, LogEventLevel> logSeverityProvider)
        {
            _sinkConfiguration = sinkConfiguration ?? throw new ArgumentNullException(nameof(sinkConfiguration));
            _discordMessageProvider = discordMessageProvider ?? throw new ArgumentNullException(nameof(discordMessageProvider));

            if (logSeverityProvider == null) throw new ArgumentNullException(nameof(logSeverityProvider));
            
            _discordWebhookClient = new DiscordWebhookClient(sinkConfiguration.WebhookUrl, new DiscordRestConfig()
            {
                LogLevel = logSeverityProvider.Provide(sinkConfiguration.LogLevel),
            });
        }

        public void PostWebhook(LogEvent logEvent)
        {
            Embed embed = _discordMessageProvider.Provide(logEvent);
            _discordWebhookClient.SendMessageAsync(
                embeds: new List<Embed>() { embed }, 
                allowedMentions: AllowedMentions.All, 
                text: logEvent.Level == LogEventLevel.Fatal && _sinkConfiguration.MentionEveryoneOnFatalLevel ? "@everyone" : null).Wait();
        }
        
        public async Task PostWebhookAsync(LogEvent logEvent)
        {
            Embed embed = _discordMessageProvider.Provide(logEvent);
            await _discordWebhookClient.SendMessageAsync(
                embeds: new List<Embed>() { embed },
                allowedMentions: AllowedMentions.None, 
                text: logEvent.Level == LogEventLevel.Fatal && _sinkConfiguration.MentionEveryoneOnFatalLevel ? "@everyone" : null);
        }

        public void Dispose() => _discordWebhookClient?.Dispose();
    }
}