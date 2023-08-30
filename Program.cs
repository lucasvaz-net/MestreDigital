using MestreDigital.DAL;
using MestreDigital.Data;
using MestreDigital.Data.Data;
using MestreDigital.Services;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adicionando o serviço de Sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Registering DatabaseConnectionService
builder.Services.AddSingleton<DatabaseConnectionService>();
builder.Services.AddTransient<CategoriaDAL>();
builder.Services.AddTransient<SubcategoriaDal>();
builder.Services.AddTransient<ConteudoDAL>();
builder.Services.AddTransient<FAQDAL>();
builder.Services.AddTransient<FeedbackDAL>();
builder.Services.AddTransient<UserStateService>();
builder.Services.AddTransient<TelegramService>();

// Register the Telegram.Bot client in the DI container.
string telegramToken = "6514778156:AAHKm6A1l0iJdz9jVcSVNcNToDso9BALCfA";
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramToken));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use Session middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Set up the webhook for the Telegram bot
var botClient = app.Services.GetRequiredService<ITelegramBotClient>();
string webhookUrl = "https://e74a-2804-3858-fc85-b600-2ea5-df4d-64a4-94e5.ngrok-free.app/api/telegram/update";
await botClient.SetWebhookAsync(webhookUrl);

app.Run();
