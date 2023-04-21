# Serilog.Sinks.Discord

Serilog.Sinks.Discord is a [Serilog](https://github.com/serilog/serilog) sink that uses Discord webhooks to submit LogEvents to a Discord channel.

| **.NET Runtime** 	| **Supported Versions**   	 |
|------------------	|----------------------------|
| .NET             	| 5, 6, 7                    |
| .NET Core        	| 3.0, 3.1     	             |
| .NET Framework   	| No	                     |

### Installation

```
PM > Install-Package Serilog.Sinks.Discord
```

# Usage

### Using the Discord sink with minimum configuration

---

Use Serilog's `LoggerConfiguration` to create your Discord sink. 
The minimum configuration required is the Discord webhook URL and the service name.
You can create the ILogger instance that submits logs to Discord as follows:

```csharp
ILogger logger = 
    new LoggerConfiguration().WriteTo.Discord(LogEventLevel.Information, config =>
    {
        config.WebhookUrl =
            "https://discord.com/api/webhooks/1234567/examplewebhookurl";
        config.ServiceName = "My Service";
    }).CreateLogger();
```
Your logger can now post logs on the Discord channel associated with the webhook URL you provided.


### Additional configuration

---

* Mention everyone on fatal event

You can choose to mention everyone observing your channel by setting the following configuration:
```csharp
config.MentionEveryoneOnFatalLevel = true;
```

**Note:** This is the default configuration. If you don't want to mention everyone on a fatal event, set the above value to `false`.

---

* Differentiate the Embed's color based on `LogEvent.Level`

You can choose whether you want the Embed to have colors associated with the `LogEvent.LogLevel` enum as follows:
```csharp
config.UseDifferentColorsOnLogLevel = true;
```

This is set to `true` by default. If you wish to use the same colors on all levels, set it to `false`.

---

* Use custom message template

You can override the default message template in two ways. One way is when configuring the Discord sink, using the `outputTemplate` parameter or by overriding the configuration in the configurator itself:

```csharp
config.OutputTemplate = "<your custom template>";
```

The default output template is set to `{Message:lj}{NewLine}{NewLine}{Exception}`. Keep in mind that Exceptions are also printed inside the Embed in a separate section.

---

* Use custom time format

You can override the timestamp format of the event, which is displayed in the Embed footer as follows:

```csharp
config.TimestampFormat = "d";
```

The default value is set to `F` which stands for full date long format. You can override it to any applicable to .NET timestamp format flags.

---

* Use an avatar for the Embed

If you'd like to style your Embed further, you can optionally add an avatar. Do so as follows:

```csharp
config.WebhookAuthorIconUrl = "<your avatar url>";
```

The default icon is set to `string.Empty` which renders no avatar.