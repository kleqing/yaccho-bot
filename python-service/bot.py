import os
import discord

from datetime import timedelta
from discord import app_commands
from dotenv import load_dotenv
from pathlib import Path

env_path = Path(__file__).parent / ".env"
load_dotenv(dotenv_path=env_path)

TOKEN = os.getenv('DISCORD_TOKEN')

intent = discord.Intents.default()

intent.message_content = True
intent.members = True # Enable the members intent to access member information

class MyBot(discord.Client):
    def __init__(self):
        super().__init__(intents=intent)
        self.tree = app_commands.CommandTree(self)

    async def setup_hook(self):
        await self.tree.sync()

bot = MyBot()

@bot.event
async def on_ready():
    print(f'{bot.user} has connected to Discord!')

# Command
@bot.tree.command(name="bomb", description="Timeout 1 người dùng")
@app_commands.describe(user="Người dùng cần timeout", duration="Thời gian timeout (giây)")
async def bomb(interaction: discord.Interaction, user: discord.Member, duration: int):
    # disable self timeout
    if user == interaction.user:
        await interaction.response.send_message("Bạn không thể timeout chính mình!", ephemeral=True)
        return

    # disable owner timeout
    if user == interaction.guild.owner:
        await interaction.response.send_message("Bạn không thể timeout owner @_@", ephemeral=True)
        return

    # role check
    if not interaction.user.guild_permissions.moderate_members:
        await interaction.response.send_message("Bạn không có quyền sử dụng lệnh này.", ephemeral=True)
        return

    # role hierarchy check
    if user.top_role >= interaction.user.top_role:
        await interaction.response.send_message("Bạn không thể timeout người dùng có vai trò cao hơn hoặc bằng bạn.", ephemeral=True)
        return

    # bot role hierarchy check
    if user.top_role >= interaction.guild.me.top_role:
        await interaction.response.send_message("Bot không thể timeout người dùng có vai trò cao hơn hoặc bằng bot.", ephemeral=True)
        return

    # timeout user
    await user.timeout(discord.utils.utcnow() + timedelta(seconds=duration))

    await interaction.response.send_message(f"{user.mention} đã bị timeout trong {duration} giây.")

bot.run(TOKEN)