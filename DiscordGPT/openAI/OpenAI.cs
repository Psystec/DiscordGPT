using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using DiscordGPT.config;

namespace DiscordGPT.openAI
{
    public class OpenAI
    {
        public static async Task<string> GetChatGptResponseAsync(string userPrompt)
        {
            if (string.IsNullOrEmpty(Config.openAIApiKey))
            {
                throw new InvalidOperationException("OpenAI API key is not set.");
            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(10);
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Config.openAIApiKey}");

                    var requestBody = new
                    {
                        model = Config.openAIModel,
                        messages = new[]
                        {
                                new { role = "system", content = Config.openAISystemPrompt },
                                new { role = "user", content = userPrompt }
                            }
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseString);

                    var returnString = jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString().Trim();

                    if (returnString == null)
                    {
                        var test = returnString;
                    }

                    if (returnString == null)
                    {
                        returnString = "ERROR CRASH: Bot is brain dead! I am a potato!";
                    }

                    return returnString;
                }
                catch (Exception ex)
                {
                    return $"ERROR: Bot is brain dead! 'I am a potato!'\n{ex.Message}";
                }
            }
        }
    }
}
