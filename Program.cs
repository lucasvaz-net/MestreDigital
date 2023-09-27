using MestreDigital.DAL;
using MestreDigital.Data;
using MestreDigital.Data.Data;
using MestreDigital.Services;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddSingleton<DatabaseConnectionService>();
builder.Services.AddTransient<CategoriaDAL>();
builder.Services.AddTransient<SubcategoriaDal>();
builder.Services.AddTransient<ConteudoDAL>();
builder.Services.AddTransient<FAQDAL>();
builder.Services.AddTransient<FeedbackDAL>();
builder.Services.AddTransient<UserStateService>();
builder.Services.AddTransient<TelegramService>();


string telegramToken = "6514778156:AAHKm6A1l0iJdz9jVcSVNcNToDso9BALCfA";
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramToken));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var botClient = app.Services.GetRequiredService<ITelegramBotClient>();
string webhookUrl = "https://mestredigital.lucasvaz.dev.br/api/telegram/update";
await botClient.SetWebhookAsync(webhookUrl);

app.Run();
 