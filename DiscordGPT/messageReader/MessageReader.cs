using DSharpPlus.EventArgs;
using DSharpPlus;
using DSharpPlus.Entities;
using DiscordGPT.common;
using DiscordGPT.openAI;

namespace DiscordGPT.messageReader
{
    public class MessageReader
    {
        public static async Task OnMessageCreated(DiscordClient bot, MessageCreateEventArgs e)
        {
            //Ignore bot messages
            if (e.Author.IsBot)
                return;

            // Get the DiscordMember object, which contains the DisplayName
            DiscordGuild discordGuild = e.Guild;
            DiscordMessage discordMessage = e.Message;
            DiscordUser discordUser = e.Author;
            DiscordMember discordMember = discordGuild.GetMemberAsync(e.Author.Id).Result;

            //Only read the message if the bot has been mentioned.
            if (!e.MentionedUsers.Contains(bot.CurrentUser))
                return;

            //Dont respond if the message sent to the bot has less that 5 chars. (This will save you OpenAI credits with stupid users.)
            var messageContent = e.Message.Content;
            if (messageContent.Length <= 5)
                return;

            //get response from OpenAI
            var response = await OpenAI.GetChatGptResponseAsync(messageContent);

            //If the response message is longer than 2000 chars, split it.
            const int maxMessageLength = 2000;
            if (response.Length > maxMessageLength)
            {
                var parts = Common.SplitIntoParts(response, maxMessageLength);
                foreach (var part in parts)
                {
                    await e.Message.RespondAsync(part);
                }
            }
            else
            {
                await e.Message.RespondAsync(response);
            }
        }
    }
}
