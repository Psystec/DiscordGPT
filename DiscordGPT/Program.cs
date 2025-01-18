using DiscordGPT.config;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DiscordGPT.messageReader;

namespace DiscordGPT
{
    internal class Program
    {
        private static DiscordClient _client { get; set; }

        async static Task Main(string[] args)
        {
            if (!await Config.LoadConfig())
                return;

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = Config.discordBotToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Error
            };
            _client = new DiscordClient(discordConfig);
            _client.Ready += Client_Ready;
            _client.MessageCreated += MessageReader.OnMessageCreated;
        }

        private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            Console.WriteLine($"Bot '{sender.CurrentUser.Username}' is up and running :)\nCreated by Psystec.");

            return Task.CompletedTask;
        }
    }
}
