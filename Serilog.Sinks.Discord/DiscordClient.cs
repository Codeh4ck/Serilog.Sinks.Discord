using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord;
using Discord.Rest;
using Discord.Webhook;
using Serilog.Events;
using Serilog.Sinks.Discord.Models;
using Serilog.Sinks.Discord.Providers;
using CommunityToolkit.HighPerformance.Buffers;

namespace Serilog.Sinks.Discord.DiscordClient
{
    public class DiscordClient
    {
        private readonly WebhookConfiguration _webhookConfiguration;
        private readonly DiscordWebhookClient _discordWebhookClient;

        private readonly IValueProvider<LogSeverity, LogEventLevel> _logSeverityProvider;
        private readonly IValueProvider<Color, LogEventLevel> _logColorProvider;

        private EmbedBuilder _embedBuilder;
        
        public DiscordClient(WebhookConfiguration webhookConfiguration, IValueProvider<LogSeverity, LogEventLevel> logSeverityProvider, IValueProvider<Color, LogEventLevel> logColorProvider)
        {
            _webhookConfiguration = webhookConfiguration ?? throw new ArgumentNullException(nameof(webhookConfiguration));
            _logSeverityProvider = logSeverityProvider ?? throw new ArgumentNullException(nameof(logSeverityProvider));
            _logColorProvider = logColorProvider ?? throw new ArgumentNullException(nameof(logColorProvider));

            _discordWebhookClient = new DiscordWebhookClient(webhookConfiguration.WebhookUrl, new DiscordRestConfig()
            {
                LogLevel = _logSeverityProvider.Provide(webhookConfiguration.LogLevel),
                UseSystemClock = true,
            });
            
            SetupEmbedBuilder();
        }

        public async Task<bool> PostWebhook(LogEvent logEvent)
        {
            ulong messageId = await _discordWebhookClient.SendMessageAsync(embeds: new List<Embed>() { BuildEmbed(logEvent) });
            return messageId != default;
        }

        private void SetupEmbedBuilder()
        {
            _embedBuilder = new EmbedBuilder();
            
            _embedBuilder.WithAuthor((authorBuilder) =>
            {
                authorBuilder.WithName(string.IsNullOrEmpty(_webhookConfiguration.WebhookAuthorUsername)
                    ? Assembly.GetExecutingAssembly().GetName().Name
                    : _webhookConfiguration.WebhookAuthorUsername);

                if (!string.IsNullOrEmpty(_webhookConfiguration.WebhookAuthorIconUrl))
                    authorBuilder.WithIconUrl(_webhookConfiguration.WebhookAuthorIconUrl);
            });
        }

        private Embed BuildEmbed(LogEvent logEvent)
        {
            _embedBuilder.Color = _logColorProvider.Provide(logEvent.Level);
            _embedBuilder.Description = logEvent.RenderMessage();
            _embedBuilder.Title = StringPool.Shared.GetOrAdd($"[{logEvent.Level:F}] - New log received");
                
            _embedBuilder.WithFooter((footerBuilder) =>
            {
                footerBuilder.WithText(logEvent.Timestamp.ToString(_webhookConfiguration.TimestampFormat));
            });
            
            return _embedBuilder.Build();
        }
    }
}