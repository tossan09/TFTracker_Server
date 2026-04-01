using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TFTDataTrackerApi.Data;
using TFTDataTrackerApi.Extensions;
using TFTDataTrackerApi.Repository;
using TFTDataTrackerApi.Security;

var builder = WebApplication.CreateBuilder(args);

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
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

