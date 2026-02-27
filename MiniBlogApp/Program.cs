using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MiniBlogApp.Services;

/**
 * @file Program.cs
 * @brief Main entry point for the MiniBlogApp application.
 * @details
 * Configures the web host, registers necessary services such as Razor Pages, 
 * UserService, BlogStorage, and LoggerService. Sets up session management 
 * and middleware, and starts the web application.
 */

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc().AddNToastNotifyToastr();

/**
 * @brief Registers Razor Pages support.
 */
builder.Services.AddRazorPages();

/**
 * @brief Configures user session management.
 * @details Налаштування сесії: тайм-аут 30 хв, лише HTTP-куки, SameSite: Lax.
 */
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

/**
 * @brief Реєстрація сервісів (Dependency Injection).
 * @details Усі сервіси додаються як Singleton, щоб дані зберігалися в пам'яті.
 */
builder.Services.AddSingleton<UserService>();

builder.Services.AddSingleton<IActivityLogger, LoggerService>();

builder.Services.AddSingleton<IBlogStorage, BlogStorage>();

var app = builder.Build();

/**
 * @brief Configures error handling and middleware pipeline.
 */
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.UseNToastNotify();

app.MapRazorPages();

/**
 * @brief Default route.
 * Redirects the root URL (/) to the login page.
 */
app.MapGet("/", () => Results.Redirect("/Login"));

app.Run();