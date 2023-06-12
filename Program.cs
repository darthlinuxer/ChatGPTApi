using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var env = builder.Environment;
if (env.IsDevelopment())
{
    builder.Configuration
        .AddJsonFile("appsettings.developer.json", optional: true, reloadOnChange: true);
}

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", builder =>
    {
        builder.WithOrigins("https://localhost:4200")
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
builder.Services.AddSwaggerGen();

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

app.Run();
