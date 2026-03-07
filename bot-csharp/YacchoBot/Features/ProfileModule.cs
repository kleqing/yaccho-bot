using Discord;
using Discord.Interactions;
using YacchoBot.Modules;

namespace YacchoBot.Features;

public class ProfileModule : BaseModule
{
    [SlashCommand("aboutyaccho", "Giới thịu về yaccho :3")]
    public async Task AboutYaccho()
    {
        var embed = new EmbedBuilder()
            .WithAuthor(
                name: "Yaccho Bot",
                iconUrl: Context.Client.CurrentUser.GetAvatarUrl()
            )
            .WithTitle("Yaoyaro các con zợ")
            .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl())
            .WithDescription(
                "Mị là Yachiyo Runami nè :3\n" +
                "Nếu mấy con zợ đã nhìn thấy mị ở đây thì xin chúc mừng các con zợ sắp toang với mị trong server này rồi đó ^.^\n\n"
            )
            
            .AddField("Credits",
                "Author: **kleqing**\n" +
                "Github: **https://github.com/kleqing/yaccho-bot**\n" +
                "Toàn bộ gif đều thuộc về anime: **Chou Kaguya-hime! (Cosmic Princess Kaguya!)**"
            )

            .WithFooter("Yaccho Bot • Made with ❤️")
            .WithColor(Color.Purple)
            .WithImageUrl("https://res.cloudinary.com/dwgjleqne/image/upload/v1772897444/about_jhdfnm.gif")
            .WithCurrentTimestamp();
        
        var components = new ComponentBuilder()
            .WithButton(label: "💖 Ủng hộ Yaccho",
                url: "https://ko-fi.com/yourpage",
                style: ButtonStyle.Link
            )
            .WithButton(
                label: "➕ Mời Yaccho vào server của bạn",
                url: "https://discord.com/oauth2/authorize?client_id=YOUR_CLIENT_ID&permissions=8&scope=bot%20applications.commands",
                style: ButtonStyle.Link
            );
            

        await RespondAsync(embed: embed.Build(), components: components.Build());
    }
}