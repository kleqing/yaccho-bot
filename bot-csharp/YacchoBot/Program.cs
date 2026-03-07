using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;
using YacchoBot.Services;

namespace YacchoBot;

class Program
{
    private IServiceProvider _services;

    static async Task Main(string[] args) => await new Program().MainAsync();

    public async Task MainAsync()
    {
        Env.Load();

        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds |
                             GatewayIntents.GuildMembers |
                             GatewayIntents.GuildMessages |
                             GatewayIntents.MessageContent
        };

        var services = new ServiceCollection()
            .AddSingleton(new DiscordSocketClient(config))
            .AddSingleton(x => new InteractionService(
                x.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<InteractionHandler>()
            .BuildServiceProvider();

        _services = services;

        var client = services.GetRequiredService<DiscordSocketClient>();

        client.Log += Log;

        string? token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("Token not found!");
            return;
        }

        await services.GetRequiredService<InteractionHandler>()
            .InitializeAsync();

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg);
        return Task.CompletedTask;
    }
}