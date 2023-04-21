using System;
using Serilog.Events;
using System.Threading.Tasks;

namespace Serilog.Sinks.Discord
{
    public interface IDiscordClient : IDisposable
    {
        /// <summary>
        /// Processes the emitted LogEvent, creates an Embed and POSTs it to the Discord webhook.
        /// </summary>
        /// <param name="logEvent">The emitted LogEvent.</param>
        void ProcessLogEvent(LogEvent logEvent);

        /// <summary>
        /// Processes the emitted LogEvent, creates an Embed and POSTs it to the Discord webhook asynchronously.
        /// </summary>
        /// <remarks>This method is currently of no use. If Serilog implements asynchronous event emission, it might come in play.</remarks>
        /// <param name="logEvent">The emitted LogEvent.</param>
        Task ProcessLogEventAsync(LogEvent logEvent);
    }
}