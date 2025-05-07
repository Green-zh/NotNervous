using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Hosting;
using NotNervous.Server.Clients;
using NotNervous.Server.Providers;

var builder = WebApplication.CreateBuilder(args);

// Enable WebSockets
builder.Services.AddWebSockets(options =>
{
    // Configure WebSocket options if needed
});
builder.Services.AddControllers();
builder.Services.AddSingleton<SpeechClient>();
builder.Services.AddSingleton<RedisClient>();
builder.Services.AddSingleton<AIFoundryClient>();
builder.Services.AddSingleton<DialogProvider>();

var app = builder.Build();
app.UseWebSockets();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();