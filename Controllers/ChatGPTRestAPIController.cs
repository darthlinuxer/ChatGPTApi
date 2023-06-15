using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Swagger;
using Swashbuckle.AspNetCore.Filters;

namespace ChatGPTApi.Controllers
{
    /// <summary>
    /// Controller to call ChatGPT API via REST
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ChatGPTRestAPIController : ControllerBase
    {

        private readonly IHttpClientFactory _factory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="factory"></param>
        public ChatGPTRestAPIController(
            IHttpClientFactory factory
            )
        {
            _factory = factory;
        }

        /// <summary>
        /// To be used for a single question and a single response
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="model"></param>
        /// <param name="max_tokens"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{model}/Reply")]
        public async Task<IActionResult> SimpleReply([FromQuery] string prompt, [FromRoute] string model = "gpt-3.5-turbo", [FromQuery] int max_tokens = 1000)
        {
            using var client = _factory.CreateClient("defaultGPT");
            var body = new
            {
                model,
                messages = new List<object>() { new { role = "user", content = prompt } },
                temperature = 0.7,
                max_tokens
            };

            var response = await client.PostAsJsonAsync(client.BaseAddress + "/v1/chat/completions", body);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                return Ok(result);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest(new { message = "API request failed", error });

        }

        /// <summary>
        /// https://platform.openai.com/docs/api-reference/completions/create
        /// </summary>
        /// <param name="completion"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [SwaggerRequestExample(typeof(Completion), typeof(CompletionsExample))]
        public async Task<IActionResult> Completions([FromBody] Completion completion)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using var client = _factory.CreateClient("defaultGPT");
            var body = completion;

            var response = await client.PostAsJsonAsync(client.BaseAddress + "/v1/chat/completions", body);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                Console.WriteLine($"Elapsed:{stopwatch.ElapsedMilliseconds}");
                return Ok(result);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest(new
            {
                message = "API request failed",
                error
            });
        }     

        /// <summary>
        /// https://platform.openai.com/docs/api-reference/images
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [SwaggerRequestExample(typeof(Image), typeof(ImageExample))]
        public async Task<IActionResult> CreateImage([FromBody] Image image)
        {
            using var client = _factory.CreateClient("defaultGPT");
            var body = image;

            var response = await client.PostAsJsonAsync(client.BaseAddress + "/v1/images/generations", body);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                return Ok(result);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest(new
            {
                message = "API request failed",
                error
            });

        }

        /// <summary>
        /// Get all openAI models
        /// </summary>
        /// <returns>List of OpenAI Models</returns> 
        [HttpGet]
        [Route("Models")]
        public async Task<IActionResult> GetModels()
        {
            using var client = _factory.CreateClient("defaultGPT");
            var response = await client.GetAsync(client.BaseAddress + "/v1/models");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                return Ok(result);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest(new { message = "API request failed", error });
        }

        /// <summary>
        /// Get Info about a Single OpenAPI model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Model/{model}")]
        public async Task<IActionResult> GetModel([FromRoute] string model)
        {
            using var client = _factory.CreateClient("defaultGPT");
            var response = await client.GetAsync(client.BaseAddress + "/v1/models/" + model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                return Ok(result);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest(new { message = "API request failed", error });
        }

    }
}