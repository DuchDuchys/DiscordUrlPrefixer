using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace DiscordUrlPrefixer;

public class Program
{
    private static DiscordSocketClient _client = null!;

    public static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var token = config["DISCORD_TOKEN"];
        if (string.IsNullOrWhiteSpace(token))
        {
            Console.Error.WriteLine("Error: DISCORD_TOKEN is not set. Set it as an environment variable or in appsettings.json.");
            return;
        }

        var socketConfig = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds
                           | GatewayIntents.GuildMessages
                           | GatewayIntents.MessageContent
        };

        _client = new DiscordSocketClient(socketConfig);
        _client.Log += LogAsync;
        _client.MessageReceived += MessageReceivedAsync;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        Console.WriteLine("Bot is running. Press Ctrl+C to stop.");

        var tcs = new TaskCompletionSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            tcs.TrySetResult();
        };
        AppDomain.CurrentDomain.ProcessExit += (_, _) => tcs.TrySetResult();

        await tcs.Task;

        await _client.StopAsync();
        Console.WriteLine("Bot stopped.");
    }

    private static Task LogAsync(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private static async Task MessageReceivedAsync(SocketMessage message)
    {
        if (message.Author.IsBot)
            return;

        var (transformedMessage, hadMatches) = UrlPrefixer.ReplaceUrls(message.Content);
        if (!hadMatches)
            return;

        try
        {
            await message.DeleteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not delete message (missing ManageMessages permission?): {ex.Message}");
            return;
        }

        await message.Channel.SendMessageAsync($"<@{message.Author.Id}>: {transformedMessage}", allowedMentions: AllowedMentions.None);
    }
}
