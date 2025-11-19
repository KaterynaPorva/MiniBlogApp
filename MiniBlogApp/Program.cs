using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

/**
 * @file Program.cs
 * @brief Main entry point for the MiniBlogApp application.
 * @details
 * Configures the web host, registers necessary services such as Razor Pages and UserService,
 * sets up session management and middleware, and starts the web application.
 * The default route redirects to the login page.
 */

var builder = WebApplication.CreateBuilder(args);

/**
 * @brief Registers Razor Pages support.
 * @details Enables handling of .cshtml page requests throughout the application.
 */
builder.Services.AddRazorPages();

/**
 * @brief Configures user session management.
 * @details
 * Session settings include:
 * - Idle timeout: 30 minutes
 * - Cookie is HTTP-only
 * - Cookie is essential
 * - SameSite policy: Lax
 * This allows tracking the current user across requests.
 */
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

/**
 * @brief Registers the UserService.
 * @details
 * UserService manages user-related operations, including authentication.
 * It is registered as a singleton to provide a single instance across the application.
 */
builder.Services.AddSingleton<MiniBlogApp.Services.UserService>();

var app = builder.Build();

/**
 * @brief Configures error handling for production environment.
 * @details
 * Uses a general error page (/Error) instead of showing detailed exceptions.
 * Also enables HTTP Strict Transport Security (HSTS).
 */
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

/** @brief Enforces HTTPS redirection. */
app.UseHttpsRedirection();

/** @brief Serves static files (CSS, JS, images, etc.). */
app.UseStaticFiles();

/** @brief Adds routing capabilities for handling requests. */
app.UseRouting();

/** @brief Enables user session support. */
app.UseSession();

/** @brief Enables authorization middleware. */
app.UseAuthorization();

/** @brief Maps Razor Pages to the request pipeline. */
app.MapRazorPages();

/**
 * @brief Default route.
 * @details Redirects the root URL (/) to the login page.
 */
app.MapGet("/", () => Results.Redirect("/Login"));

/** @brief Starts the web application and begins listening for requests. */
app.Run();
