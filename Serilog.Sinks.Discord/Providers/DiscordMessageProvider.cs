﻿using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.Discord.Models;

namespace Serilog.Sinks.Discord.Providers

{
    /// <summary>
    /// The default Discord message provider that transforms a <see cref="LogEvent"/> to an <see cref="Embed"/>.
    /// </summary>
    public class DiscordMessageProvider : IDiscordMessageProvider
    {
        private readonly SinkConfiguration _sinkConfiguration;
        private readonly IValueProvider<Color, LogEventLevel> _logColorValueProvider;
        private readonly ITextFormatter _textFormatter;

        private EmbedBuilder _embedBuilder;

        internal DiscordMessageProvider(SinkConfiguration sinkConfiguration, IValueProvider<Color, LogEventLevel> logColorValueProvider)
        {
            _sinkConfiguration = sinkConfiguration ?? throw new ArgumentNullException(nameof(sinkConfiguration));
            _logColorValueProvider = logColorValueProvider ?? throw new ArgumentNullException(nameof(logColorValueProvider));

            _textFormatter = new MessageTemplateTextFormatter(sinkConfiguration.OutputTemplate);

            SetupEmbedBuilder();
        }

        public Embed Provide(LogEvent logEvent)
        {
            if (_sinkConfiguration.UseDifferentColorsOnLogLevel)
                _embedBuilder.WithColor(_logColorValueProvider.Provide(logEvent.Level));

            _embedBuilder.WithTitle($"[{logEvent.Level:F}]");

            string message;

            using (StringWriter writer = new StringWriter())
            {
                _textFormatter.Format(logEvent, writer);
                message = writer.ToString();
            }

            StringBuilder descriptionBuilder = new StringBuilder();

            descriptionBuilder.AppendLine("\n");
            descriptionBuilder.AppendLine("\n**Log Message:**");

            descriptionBuilder.Append("```");
            descriptionBuilder.AppendLine(message);
            descriptionBuilder.Append("```");

            if (logEvent.Exception != null)
            {
                descriptionBuilder.AppendLine($"\n**Exception Type:**");
                descriptionBuilder.AppendLine("```");
                descriptionBuilder.AppendLine(logEvent.Exception.GetType().FullName);
                descriptionBuilder.AppendLine("```");
                descriptionBuilder.AppendLine("**Exception Message:**");
                descriptionBuilder.AppendLine("```");
                descriptionBuilder.AppendLine(logEvent.Exception.Message);
                descriptionBuilder.AppendLine("```");
            }

            if (logEvent.Properties.Count > 0)
            {
                descriptionBuilder.AppendLine("\n**Parameters:**");

                //if (CanBuildFields(logEvent))
                //    _embedBuilder.WithFields(ParseLogProperties(logEvent));
                //else

                ParseLogPropertiesAsString(logEvent, descriptionBuilder);
            }

            _embedBuilder.WithDescription(descriptionBuilder.ToString());
            _embedBuilder.WithFooter((footerBuilder) =>
            {
                footerBuilder.WithText(logEvent.Timestamp.ToString(_sinkConfiguration.TimestampFormat));
            });

            return _embedBuilder.Build();
        }

        private void SetupEmbedBuilder()
        {
            _embedBuilder = new EmbedBuilder();

            _embedBuilder.WithAuthor((authorBuilder) =>
            {
                authorBuilder.WithName(string.IsNullOrEmpty(_sinkConfiguration.ServiceName)
                    ? Assembly.GetEntryAssembly()?.GetName().Name ?? "Serilog"
                    : _sinkConfiguration.ServiceName);

                if (!string.IsNullOrEmpty(_sinkConfiguration.WebhookAuthorIconUrl))
                    authorBuilder.WithIconUrl(_sinkConfiguration.WebhookAuthorIconUrl);
            });
        }

        //private List<EmbedFieldBuilder> ParseLogProperties(LogEvent logEvent)
        //{
        //    var fields = logEvent.Properties;

        //    List<EmbedFieldBuilder> fieldBuilders = new List<EmbedFieldBuilder>(logEvent.Properties.Count);

        //    foreach (var field in fields)
        //    {
        //        EmbedFieldBuilder builder = new EmbedFieldBuilder();
        //        builder.WithName(field.Key);

        //        using (StringWriter writer = new StringWriter())
        //        {
        //            field.Value.Render(writer);
        //            builder.WithValue(writer.ToString());
        //        }

        //        fieldBuilders.Add(builder);
        //    }

        //    return fieldBuilders;
        //}

        private void ParseLogPropertiesAsString(LogEvent logEvent, StringBuilder builder)
        {
            var fields = logEvent.Properties;

            builder.AppendLine("```");

            foreach (var field in fields)
            {
                using StringWriter writer = new StringWriter();

                field.Value.Render(writer);
                builder.AppendLine($"[{field.Key}]: {writer.ToString()}");
                builder.AppendLine();
            }

            builder.AppendLine("```");
        }

        private bool CanBuildFields(LogEvent logEvent)
        {
            var fields = logEvent.Properties;
            if (fields.Count > 25) return false;

            return !fields.Any(x => x.Key.Length > 256);
        }
    }
}