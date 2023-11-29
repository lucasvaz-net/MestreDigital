using MestreDigital.DAL;
using MestreDigital.Data;
using MestreDigital.Filters;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var connectionString = builder.Configuration.GetConnectionString("MestreDigitalDb");

builder.Services.AddSingleton<DatabaseConnectionService>();
builder.Services.AddTransient<CategoriaDAL>();
builder.Services.AddTransient<SubcategoriaDal>();
builder.Services.AddTransient<ConteudoDAL>();
builder.Services.AddTransient<FAQDAL>();
builder.Services.AddTransient<FeedbackDAL>();
builder.Services.AddScoped<UserTokenDal>(sp => new UserTokenDal(connectionString)); 
builder.Services.AddScoped<TokenAuthorizeAttribute>();
builder.Services.AddTransient<UsuarioDal>();
builder.Services.AddTransient<HistoriaDAL>();

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
//string webhookUrl = "https://442d-2804-3858-fc2c-cd00-1cba-dcae-6879-6a56.ngrok-free.app/api/telegram/update";
await botClient.SetWebhookAsync(webhookUrl);

app.Run();
 