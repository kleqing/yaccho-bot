using Discord.Interactions;
using Discord.WebSocket;

namespace YacchoBot.Modules;

public abstract class BaseModule : InteractionModuleBase<SocketInteractionContext>
{
    protected SocketGuildUser Invoker => (SocketGuildUser)Context.User;
    protected SocketGuild Guild => Context.Guild;
    protected SocketGuildUser BotUser => Guild.CurrentUser;
    protected async Task RespondError(string message)
    {
        await RespondAsync(message, ephemeral: true);
    }
}