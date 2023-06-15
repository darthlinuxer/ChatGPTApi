using Models;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger;
/// <summary>
/// Swagger Example
/// </summary>
public class CompletionsExample : IMultipleExamplesProvider<Completion>
{
    public IEnumerable<SwaggerExample<Completion>> GetExamples()
    {
        yield return SwaggerExample.Create<Completion>("Minimum", new Completion
        {
            Messages = new List<Message> {
                new Message{Role="user",Content="Hi, what would could you say about ChatGPT future?"},
            },
            Max_tokens = 500,
        });

        yield return SwaggerExample.Create<Completion>("Full", new Completion
        {
            Model = "gpt-3.5-turbo",
            Messages = new List<Message> {
                new Message{Role="user",Content="Hi, who won the soccer world cup in 1970?"},
                new Message{Role="user",Content="Who was the best player?"}
            },
            Max_tokens = 500,
            Temperature = 0.5,
            Top_p = 1,
            N = 1,
            Stop = null,
            Presence_penalty = 0,
            Frequency_penalty = 0,
            User = "ChatGPTApi"
        });

         yield return SwaggerExample.Create<Completion>("Stream", new Completion
        {
            Model = "gpt-3.5-turbo",
            Messages = new List<Message> {
                new Message{Role="user",Content="Hi, who won the soccer world cup in 1970?"},
                new Message{Role="user",Content="Who was the best player?"}
            },
            Max_tokens = 500,
            Temperature = 0.5,
            Stream = true,
            Top_p = 1,
            N = 1,
            Stop = null,
            Presence_penalty = 0,
            Frequency_penalty = 0,
            User = "ChatGPTApi"
        });
    }
}