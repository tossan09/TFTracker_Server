using TFTDataTrackerApi.Data;
using TFTDataTrackerApi.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddScoped<CompRepository>();
builder.Services.AddScoped<MatchRepository>();
builder.Services.AddScoped<SetRepository>();
builder.Services.AddScoped<PatchRepository>();

builder.Services.AddScoped<DbContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

