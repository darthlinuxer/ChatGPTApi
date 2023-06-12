using System.Net.Http.Headers; // Add this line
using Microsoft.AspNetCore.Mvc;
using Models;
using Swagger;
using Swashbuckle.AspNetCore.Filters;

namespace ChatGPTApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatGPTController : ControllerBase
    {

        private readonly IHttpClientFactory _factory;

        public ChatGPTController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        [HttpGet]
        [Route("{model}/Reply")]
        public async Task<IActionResult> SimpleReply([FromQuery] string prompt, [FromRoute] string model = "gpt-3.5-turbo")
        {
            using var client = _factory.CreateClient("defaultGPT");
            var body = new
            {
                model = model,
                messages = new List<object>() { new { role = "user", content = prompt } },
                temperature = 0.7
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
        [Route("Completions")]
        [SwaggerRequestExample(typeof(Completion), typeof(CompletionsExample))]
        public async Task<IActionResult> Completions([FromBody] Completion completion)
        {
            using var client = _factory.CreateClient("defaultGPT");
            var body = completion;

            var response = await client.PostAsJsonAsync(client.BaseAddress + "/v1/chat/completions", body);

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
        /// https://platform.openai.com/docs/api-reference/completions/create
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