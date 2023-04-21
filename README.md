# Serilog.Sinks.Discord

Serilog.Sinks.Discord is a [Serilog](https://github.com/serilog/serilog) sink that uses Discord webhooks to submit LogEvents to a Discord channel.

| **.NET Runtime** 	| **Supported Versions**   	 |
|------------------	|----------------------------|
| .NET             	| 5, 6, 7                    |
| .NET Core        	| 3.0, 3.1     	             |
| .NET Framework   	| No	                     |

### Installation

```
PM > Install-Package Codelux.Serilog.Sinks.Discord
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


## Additional configuration

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

# Contributing

## Found an issue?

Please report any issues you have found by [creating a new issue](https://github.com/Codeh4ck/Serilog.Sinks.Discord/issues). We will review the case and if it is indeed a problem with the code, I will try to fix it as soon as possible. I want to maintain a healthy and bug-free standard for our code. Additionally, if you have a solution ready for the issue please submit a pull request.

## Submitting pull requests

Before submitting a pull request to the repository please ensure the following:

* Your code follows the naming conventions [suggested by Microsoft](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines)
* Your code works flawlessly, is fault tolerant and it does not break the library or aspects of it
* Your code follows proper object oriented design principles. Use interfaces!

Your code will be reviewed and if it is found suitable it will be merged. Please understand that the final decision always rests with me. By submitting a pull request you automatically agree that I hold the right to accept or deny a pull request based on my own criteria.

## Contributor License Agreement

By contributing your code to **Serilog.Sinks.Discord** you grant Nikolas Andreou a non-exclusive, irrevocable, worldwide, royalty-free, sublicenseable, transferable license under all of Your relevant intellectual property rights (including copyright, patent, and any other rights), to use, copy, prepare derivative works of, distribute and publicly perform and display the Contributions on any licensing terms, including without limitation: (a) open source licenses like the MIT license; and (b) binary, proprietary, or commercial licenses. Except for the licenses granted herein, You reserve all right, title, and interest in and to the Contribution.

You confirm that you are able to grant us these rights. You represent that you are legally entitled to grant the above license. If your employer has rights to intellectual property that you create, You represent that you have received permission to make the contributions on behalf of that employer, or that your employer has waived such rights for the contributions.

You represent that the contributions are your original works of authorship and to your knowledge, no other person claims, or has the right to claim, any right in any invention or patent related to the contributions. You also represent that you are not legally obligated, whether by entering into an agreement or otherwise, in any way that conflicts with the terms of this license.

Nikolas Andreou acknowledges that, except as explicitly described in this agreement, any contribution which you provide is on an "as is" basis, without warranties or conditions of any kind, either express or implied, including, without limitation, any warranties or conditions of title, non-infringement, merchantability, or fitness for a particular purpose.