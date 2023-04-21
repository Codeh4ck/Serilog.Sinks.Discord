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