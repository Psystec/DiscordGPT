using Newtonsoft.Json;

namespace DiscordGPT.config
{
    public static class Config
    {
        public static string discordBotToken { get; set; }
        public static string openAIApiKey { get; set; }
        public static string openAIModel { get; set; }
        public static string openAISystemPrompt { get; set; }


        private static readonly string configFile = "config.json";

        public static async Task<bool> LoadConfig()
        {
            try
            {
                if (!File.Exists(configFile))
                {
                    var defaultConfig = JsonConvert.SerializeObject(new Configuration(), Formatting.Indented);
                    File.WriteAllText(configFile, defaultConfig);
                    Console.WriteLine("Configuration file created. Please close the bot, update the config.json file, and restart the bot.");
                    return false;
                }

                string configContent = await File.ReadAllTextAsync(configFile);
                var configData = JsonConvert.DeserializeObject<Configuration>(configContent);

                if (configData == null)
                {
                    Console.WriteLine("The configuration file could not be deserialized. Ensure it contains valid JSON.");
                    return false;
                }

                (discordBotToken, openAIApiKey, openAIModel, openAISystemPrompt) =
                    (configData.discordBotToken, configData.openAIApiKey, configData.openAIModel, configData.openAISystemPrompt);

                var missingFields = new List<string>();
                if (string.IsNullOrWhiteSpace(discordBotToken)) missingFields.Add(nameof(discordBotToken));
                if (string.IsNullOrWhiteSpace(openAIApiKey)) missingFields.Add(nameof(openAIApiKey));
                if (string.IsNullOrWhiteSpace(openAIModel)) missingFields.Add(nameof(openAIModel));
                if (string.IsNullOrWhiteSpace(openAISystemPrompt)) missingFields.Add(nameof(openAISystemPrompt));

                if (missingFields.Any())
                {
                    Console.WriteLine($"The following configuration fields cannot be null or empty: {string.Join(", ", missingFields)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading the configuration file: {ex.Message}");
                return false;
            }
        }

        private class Configuration
        {
            public string discordBotToken { get; set; }
            public string openAIApiKey { get; set; }
            public string openAIModel { get; set; }
            public string openAISystemPrompt { get; set; }
        }
    }
}
