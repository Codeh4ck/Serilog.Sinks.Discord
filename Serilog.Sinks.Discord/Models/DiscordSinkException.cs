using System;

namespace Serilog.Sinks.Discord.Models
{
    /// <summary>
    /// Represents an exception thrown by <see cref="Serilog.Sinks.Discord"/>
    /// </summary>
    public class DiscordSinkException : Exception
    {
        public DiscordSinkException() { }
        public DiscordSinkException(string message) : base(message) {}
        public DiscordSinkException(string message, Exception innerException) : base(message, innerException) {}
    }
}