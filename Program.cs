using System.Net.Http.Headers;
using System.Reflection;
using ChatGPTApi.Controllers;
using ChatGPTApi.SignalR;
using Microsoft.OpenApi.Models;
using Swagger;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var env = builder.Environment;
if (env.IsDevelopment())
{
    builder.Configuration
        .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true);
}

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
    options.AddPolicy("AngularHttps", builder =>
    {
        builder.WithOrigins("https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
    options.AddPolicy("Any", builder =>
    {
       builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddHttpClient("defaultGPT", options =>
{
    options.BaseAddress = new Uri("https://api.openai.com");
    var api = configuration.GetValue<string>("OpenAIKey");
    if (string.IsNullOrEmpty(api)) throw new NullReferenceException("OpenAPIKey cannot be null or empty");
    options.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", api);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ChatGPT API",
        Description = "A .NET Core Web API for ChatGPT",
        Contact = new OpenApiContact
        {
            Name = "Camilo Chaves",
            Url = new Uri("https://linkedin.com/in/camilochaves")
        },
        License = new OpenApiLicense
        {
            Name = "Gnu License",
            Url = new Uri("https://www.gnu.org/licenses/agpl-3.0.txt")
        }
    });

    options.ExampleFilters();
    // Include XML comments if available
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<CompletionsExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ImageExample>();
builder.Services.AddSignalR();
builder.Services.AddScoped<ChatGPTRestAPIController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatGPTHub>("/ChatGPTHub");

app.Run();
