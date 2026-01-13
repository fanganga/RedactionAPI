using RedactionAPI.Services;
using RedactionAPI.Utilities.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICustomLogger, FileLogger>();
builder.Services.AddSingleton<IRedactService, RedactService>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
