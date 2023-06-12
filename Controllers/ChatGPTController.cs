using System.Net.Http.Headers; // Add this line
using Microsoft.AspNetCore.Mvc;

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
        [Route("SimpleReply")]
        public async Task<IActionResult> SimpleReply([FromQuery] string prompt)
        {
            using var client = _factory.CreateClient("defaultGPT");
            var body = new
            {
                model = "gpt-3.5-turbo",
                messages = new List<object>(){new {role="user", content=prompt}},
                temperature = 0.7
            };

            var response = await client.PostAsJsonAsync(client.BaseAddress+"/v1/chat/completions", body);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                return Ok(result);
            }

            var error = await response.Content.ReadAsStringAsync();
            return BadRequest(new { message = "API request failed", error });

        }

        [HttpGet]
        [Route("Models")]
        public async Task<IActionResult> GetModels()
        {
            using var client = _factory.CreateClient("defaultGPT");
            var response = await client.GetAsync(client.BaseAddress+"/v1/models");

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