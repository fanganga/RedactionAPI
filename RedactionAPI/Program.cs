using RedactionAPI.Services;
using RedactionAPI.Utilities.Logging;

var builder = WebApplication.CreateBuilder([]);

int port = 8080;
if(args.Length > 0)
{
    int.TryParse(args[0], out port );
}
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(System.Net.IPAddress.Parse("127.0.0.1"), port);
});
// Add services to the container.
builder.Services.AddScoped<ICustomLogger, FileLogger>();
builder.Services.AddSingleton<IInformationService, InformationService>();
builder.Services.AddSingleton<IRedactService, RedactService>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
