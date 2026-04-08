using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TFTDataTrackerApi.Data;
using TFTDataTrackerApi.Extensions;
using TFTDataTrackerApi.Repository;
using TFTDataTrackerApi.Security;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/tftracker_log.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss}\nLevel: {Level:u3}\nUser: {User}\nRoles: {Roles}\nMessage: {Message:lj}\nException: {Exception}\n---\n")
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddScoped<CompRepository>();
builder.Services.AddScoped<MatchRepository>();
builder.Services.AddScoped<SetRepository>();
builder.Services.AddScoped<PatchRepository>();
builder.Services.AddScoped<StatsRepository>();
builder.Services.AddScoped<DbContext>();

builder.Services.AddScoped<TokenService>();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173", "http://tftracker.com.br:7032")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    if (builder.Environment.IsDevelopment())
    {
        endpoints.MapFallback(async ctx =>
        {
            ctx.Response.Redirect("https://localhost:3000/");
            await Task.Yield();
        });
    }
    else
    {
        endpoints.MapFallbackToFile("/index.html");
    }
});

app.Run();
