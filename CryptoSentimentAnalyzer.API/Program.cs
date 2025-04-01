// CryptoSentimentAnalyzer.API/Program.cs
using CryptoSentimentAnalyzer.API.Hubs;
using CryptoSentimentAnalyzer.API.Services;
using CryptoSentimentAnalyzer.Application.Features.Coins.Commands;
using CryptoSentimentAnalyzer.Application.Interfaces;
using CryptoSentimentAnalyzer.Infrastructure.Data;
using CryptoSentimentAnalyzer.Infrastructure.Data.Repositories;
using CryptoSentimentAnalyzer.Infrastructure.MessageBroker;
using CryptoSentimentAnalyzer.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy => policy.WithOrigins("http://localhost:3000") // Adjust for your React app
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Add controllers and API explorer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR
builder.Services.AddSignalR();

// Add database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

// Add repositories
builder.Services.AddScoped<ICoinRepository, CoinRepository>();
builder.Services.AddScoped<ITweetRepository, TweetRepository>();
builder.Services.AddScoped<ISentimentResultRepository, SentimentResultRepository>();
builder.Services.AddScoped<ISentimentSummaryRepository, SentimentSummaryRepository>();

// Add services
builder.Services.AddScoped<ISentimentAnalysisService, SentimentAnalysisService>();
builder.Services.AddScoped<ITwitterService, TwitterService>();
builder.Services.AddSingleton<IMessageBroker, RabbitMQMessageBroker>();

// Add MediatR
builder.Services.AddMediatR(Assembly.GetExecutingAssembly(),
    typeof(AnalyzeCoinSentimentCommand).Assembly);

// Add background services
builder.Services.AddHostedService<SentimentUpdateService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();
app.MapHub<SentimentHub>("/sentimentHub");

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
        // Log the full exception details
    }
}

app.Run();