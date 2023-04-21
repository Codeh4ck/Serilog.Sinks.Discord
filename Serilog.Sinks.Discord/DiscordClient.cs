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
    /// <summary>
    /// A Discord client that transforms a <see cref="LogEvent"/> to an <see cref="Embed"/> and POSTs it to a given webhook.
    /// </summary>
    public class DiscordClient : IDiscordClient
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

        
        /// <summary>
        /// Processes the emitted LogEvent, creates an Embed and POSTs it to the Discord webhook.
        /// </summary>
        /// <param name="logEvent">The emitted LogEvent.</param>
        public void ProcessLogEvent(LogEvent logEvent)
        {
            Embed embed = _discordMessageProvider.Provide(logEvent);
            
            _discordWebhookClient.SendMessageAsync(
                embeds: new List<Embed>() { embed }, 
                allowedMentions: AllowedMentions.All, 
                text: logEvent.Level == LogEventLevel.Fatal && _sinkConfiguration.MentionEveryoneOnFatalLevel ? "@everyone" : null).Wait();
        }
        
        /// <summary>
        /// Processes the emitted LogEvent, creates an Embed and POSTs it to the Discord webhook asynchronously.
        /// </summary>
        /// <remarks>This method is currently of no use. If Serilog implements asynchronous event emission, it might come in play.</remarks>
        /// <param name="logEvent">The emitted LogEvent.</param>
        public async Task ProcessLogEventAsync(LogEvent logEvent)
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