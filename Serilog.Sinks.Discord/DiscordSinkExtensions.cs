using System;
using Discord;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Discord.Models;
using Serilog.Sinks.Discord.Providers;

namespace Serilog.Sinks.Discord
{
    public static class DiscordSinkExtensions
    {
        private static readonly SinkConfiguration SinkConfiguration = new SinkConfiguration();

        /// <summary>
        /// Writes log events to a Discord channel by submitting the data to a webhook.
        /// </summary>
        /// <param name="config">Serilog's Discord sink configuration.</param>
        /// <param name="configurator">The sink's logging configuration delegate.</param>
        /// <param name="minimumLogLevel">The minimum level for events passed through the sink.</param>
        /// <param name="outputTemplate">A message template describing the format used to write to the sink. The default is <code>"{Message:lj}{NewLine}{NewLine}{Exception}"</code>.</param>
        /// <param name="discordClient">The client which performs all communication with Discord's API. Override this to create your own Discord client that processes <see cref="LogEvent"/> differently.</param>
        /// <param name="discordMessageProvider">The provider that transforms <see cref="LogEvent"/> to <see cref="Embed"/>. Override this parameter to set a custom provider that creates a Discord Embed with your own styling.</param>
        /// <returns></returns>
        public static LoggerConfiguration Discord(
            this LoggerSinkConfiguration config,
            LogEventLevel minimumLogLevel,
            Action<SinkConfiguration> configurator,
            string outputTemplate = "{Message:lj}{NewLine}{NewLine}{Exception}",
            IDiscordClient discordClient = null,
            IValueProvider<Embed, LogEvent> discordMessageProvider = null)
        {
            configurator(SinkConfiguration);

            SinkConfiguration.OutputTemplate = outputTemplate;
            SinkConfiguration.LogLevel = minimumLogLevel;

            ValidateConfiguration();

            if (discordClient != null && discordMessageProvider != null)
            {
                throw new DiscordSinkException(
                    $"{nameof(discordMessageProvider)} can only be set when the default Discord client is used. " +
                    $"Inject your custom {nameof(IValueProvider<Embed, LogEvent>)} in your {nameof(IDiscordClient)} implementation instead.");
            }
            
            IDiscordClient client = discordClient;

            if (discordClient == null)
            {
                IValueProvider<Embed, LogEvent> messageProvider = discordMessageProvider ?? new DiscordMessageProvider(SinkConfiguration, ILogColorValueProvider.Default.Value);
                client = new DiscordClient(SinkConfiguration, messageProvider, ILogLevelValueProvider.Default.Value);
            }
            
            return config.Sink(new DiscordSink(client, SinkConfiguration));
        }

        private static void ValidateConfiguration()
        {
            if (string.IsNullOrEmpty(SinkConfiguration.WebhookUrl))
                throw new DiscordSinkException($"{nameof(Models.SinkConfiguration.WebhookUrl)} cannot be empty.");

            if (string.IsNullOrEmpty(SinkConfiguration.OutputTemplate))
                throw new DiscordSinkException($"{nameof(Models.SinkConfiguration.OutputTemplate)} cannot be empty.");

            if (string.IsNullOrEmpty(SinkConfiguration.ServiceName))
                throw new DiscordSinkException($"{nameof(Models.SinkConfiguration.ServiceName)} cannot be empty.");
        }
    }
}