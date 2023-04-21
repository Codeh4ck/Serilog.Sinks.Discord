using Serilog.Events;

namespace Serilog.Sinks.Discord.Models
{
    /// <summary>
    /// Represents configuration values for the Discord client.
    /// </summary>
    public class SinkConfiguration
    {
        /// <summary>
        /// The Discord webhook URL.
        /// </summary>
        public string WebhookUrl { get; set; }

        /// <summary>
        /// The minimum log level configuration.
        /// </summary>
        public LogEventLevel LogLevel { get; set; } = LogEventLevel.Verbose;

        /// <summary>
        /// Set to true to differentiate the message coloring based on <see cref="LogLevel"/>.
        /// </summary>
        public bool UseDifferentColorsOnLogLevel { get; set; } = true;

        /// <summary>
        /// If set, will override the default service name (Serilog Discord Sink) which is used as the embed's title.
        /// </summary>
        public string WebhookServiceName { get; set; } = "Serilog Discord Sink";

        /// <summary>
        /// Set the avatar URL of the webhook author.
        /// </summary>
        public string WebhookAuthorIconUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// The timestamp format as represented in the webhook Embed footer.
        /// </summary>
        public string TimestampFormat { get; set; } = "F";
    }
}