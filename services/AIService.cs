using System.Text.Json;
using OpenAI;
using OpenAI.Chat;
using TaskManagerApi.Model;

namespace TaskManagerApi.services
{
    public interface IAIService
    {
        Task<TaskSuggestion> GenerateTaskSuggestion(string taskDescription);

    }
    public class AIService : IAIService
    {
        private readonly OpenAIClient _client;
        public AIService(IConfiguration configuration)
        {
            _client = new OpenAIClient(configuration["OpenAI:ApiKey"]);
        }
        public async Task<TaskSuggestion> GenerateTaskSuggestion(string description)
        {
            var prompt = $@"
            Analyze this task:

            {description}

            Return ONLY valid JSON.

            The JSON must follow exactly this structure:

            {{
              ""priority"": ""low | medium | high"",
              ""subtasks"": [
                {{
                  ""name"": ""string"",
                  ""description"": ""string"",
                  ""estimated_time_hours"": number
                }}
              ],
              ""total_estimated_time_hours"": number
            }}

            Rules:
            - Every subtask MUST contain all three fields:
              name, description, estimated_time_hours.
            - Never omit the description field.
            - The description must explain what the subtask involves.
            - estimated_time_hours can be a decimal number (example: 1.5).
            - Do not add extra fields.
            - Do not use markdown code blocks.
            - Do not include ```json.
            - Start directly with {{ and end directly with }}.

            Example subtask:

            {{
              ""name"": ""Research company and role"",
              ""description"": ""Learn about company background, role responsibilities, required skills, and expectations."",
              ""estimated_time_hours"": 2
            }}
            ";
            var response = await _client.GetChatClient("gpt-4.1-mini").CompleteChatAsync([new UserChatMessage(prompt)]);
            Console.WriteLine($"AI Raw Response: {response.Value.Content[0].Text}");
            var aiText = response.Value.Content[0].Text;
            Console.WriteLine($"AI Response: {aiText}");
            aiText = aiText.Replace("```json", "")
                .Replace("```", "")
                .Trim(); // Remove any leading/trailing backticks
            var suggestion = JsonSerializer.Deserialize<TaskSuggestion>(aiText,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return suggestion;
        }
    }
}
