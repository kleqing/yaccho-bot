using Discord.Interactions;
using Discord.WebSocket;

namespace YacchoBot.Services;

public class InteractionHandler
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactions;
    private readonly IServiceProvider _services;
    private ulong guidId = 1240223749488508969;
    
    public InteractionHandler(DiscordSocketClient client, InteractionService interactions, IServiceProvider services)
    {
        _client = client;
        _interactions = interactions;
        _services = services;
    }
    
    public async Task InitializeAsync()
    {
        _client.Ready += ReadyAsync;
        _client.InteractionCreated += HandleInteractionAsync;
        
        await _interactions.AddModulesAsync(assembly: typeof(InteractionHandler).Assembly, services: _services);
    }

    private async Task ReadyAsync()
    {
        await _interactions.RegisterCommandsToGuildAsync(guidId);
    }
    
    private async Task HandleInteractionAsync(SocketInteraction interaction)
    {
        var ctx = new SocketInteractionContext(_client, interaction);
        await _interactions.ExecuteCommandAsync(ctx, _services);
    }
}