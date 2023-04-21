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
        public string ServiceName { get; set; } = "Serilog Discord Sink";

        /// <summary>
        /// Set the avatar URL of the webhook author.
        /// </summary>
        public string WebhookAuthorIconUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// The timestamp format as represented in the webhook Embed footer.
        /// </summary>
        public string TimestampFormat { get; set; } = "F";
        
        /// <summary>
        /// A message template describing the format used to write to the sink. The default is <code>"{Message:lj}{NewLine}{NewLine}{Exception}"</code>.
        /// </summary>
        public string OutputTemplate { get; set; }

        /// <summary>
        /// When set to true, the Discord message containing the log will mention everyone on the server when an event of <seealso cref="LogEventLevel.Fatal"/> is fired./>
        /// </summary>
        public bool MentionEveryoneOnFatalLevel { get; set; } = true;
    }
}