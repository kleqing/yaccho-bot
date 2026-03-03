import os
import discord

from dotenv import load_dotenv
from pathlib import Path

env_path = Path(__file__).parent / ".env"
load_dotenv(dotenv_path=env_path)

TOKEN = os.getenv("DISCORD_TOKEN")

intents = discord.Intents.default()
intents.message_content = True
intents.members = True

class MyBot(discord.Client):
    def __init__(self):
        super().__init__(intents=intents);
        self.reply_mode = False

bot = MyBot()

async def reply_message(message: str) -> str:
    return f"Bạn đã gửi: {message}"

@bot.event
async def on_ready():
    print(f'{bot.user} has connected to Discord!')

@bot.event
async def on_message(message: discord.Message):
    if message.author.bot:
        return

    content = message.content.lower()

    if content == "!replysmart on":
        bot.reply_mode = True
        await message.channel.send("Đã bật AI reply.")
        return

    if content == "!replysmart off":
        bot.reply_mode = False
        await message.channel.send("Đã tắt AI reply.")
        return

    is_mentioned = bot.user in message.mentions

    if bot.reply_mode or is_mentioned:
        cleaned_content = message.content.replace(f"<@{bot.user.id}>", "").strip()

        if not cleaned_content:
            return

        reply = await reply_message(cleaned_content)
        await message.reply(reply)

bot.run(TOKEN)