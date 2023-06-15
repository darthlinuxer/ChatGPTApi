using System.ComponentModel.DataAnnotations;

namespace Models;

/// <summary>
/// Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of alternative tokens at each position. Creates a completion for the provided prompt and parameters.
/// </summary>
public class Completion
{

    /// <summary>
    /// ID of the model to use. You can use the List models API to see all of your available models, or see our Model overview for descriptions of them.
    /// </summary>
    /// <value>gpt-3.5-turbo</value>
    [Required]
    public string Model { get; set; } = "gpt-3.5-turbo";

    /// <summary>
    /// A list of messages describing the conversation so far.
    /// </summary>
    /// <typeparam name="Message"></typeparam>
    /// <returns></returns>
    [Required]
    public List<Message> Messages { get; set; } = new List<Message>();
    /// <summary>
    /// What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic.
    /// </summary>
    /// <value>1</value>
    public double Temperature { get; set; } = 1;
    /// <summary>
    /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are considered. We generally recommend altering this or temperature but not both.
    /// </summary>
    /// <value>1</value>
    public int Top_p { get; set; } = 1;
    /// <summary>
    /// How many completions to generate for each prompt.Note: Because this parameter generates many completions, it can quickly consume your token quota. Use carefully and ensure that you have reasonable settings for max_tokens and stop.
    /// </summary>
    /// <value>1</value>
    public int N { get; set; } = 1;
    /// <summary>
    /// Whether to stream back partial progress. If set, tokens will be sent as data-only server-sent events as they become available, with the stream terminated by a data: [DONE] message.
    /// </summary>
    /// <value>false</value>
    public bool Stream { get; set;} = false;
    /// <summary>
    /// Up to 4 sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.
    /// </summary>
    /// <typeparam name="string"></typeparam>
    /// <returns></returns>
    public List<string>? Stop { get; set; } = null;

    /// <summary>
    /// The maximum number of tokens to generate in the completion. The token count of your prompt plus max_tokens cannot exceed the model's context length
    /// </summary>
    /// <value>16</value>
    public int Max_tokens { get; set; } = 100;
    /// <summary>
    /// Number between -2.0 and 2.0. Positive values penalize new tokens based on whether they appear in the text so far, increasing the model's likelihood to talk about new topics.
    /// </summary>
    /// <value>0 or null</value>
    public int? Presence_penalty { get; set; } = 0;
    /// <summary>
    /// Number between -2.0 and 2.0. Positive values penalize new tokens based on their existing frequency in the text so far, decreasing the model's likelihood to repeat the same line verbatim.
    /// </summary> 
    /// <value>0 or null</value>
    public int? Frequency_penalty { get; set; } = 0;


    /// <summary>
    /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
    /// </summary>
    /// <value>ChatGPTApi</value>
    public string User { get; set; } = "ChatGPTApi";

}