using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using MiniBlogApp.Services;
using MiniBlogApp.Builders;
using MiniBlogApp.Facades;
using MiniBlogApp.Observers;
using System;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages + Toast
builder.Services.AddMvc().AddNToastNotifyToastr();
builder.Services.AddRazorPages();

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});


// =======================
// 🔹 DEPENDENCY INJECTION
// =======================

// Базові сервіси
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<IActivityLogger, LoggerService>();

// ✅ 1. Реєструємо базове сховище
builder.Services.AddSingleton<BlogStorage>();

// ✅ 2. Декоратор (Decorator Pattern)
builder.Services.AddSingleton<IBlogStorage>(provider =>
{
    var baseStorage = provider.GetRequiredService<BlogStorage>();
    var logger = provider.GetRequiredService<IActivityLogger>();

    return new LoggingBlogStorageDecorator(baseStorage, logger);
});

// Адаптер (Adapter Pattern)
builder.Services.AddSingleton<IMarkdownParser, MarkdigAdapter>();

// Будівельник (Builder Pattern)
builder.Services.AddTransient<IPostBuilder, PostBuilder>();

// Фасад + Observer (Facade + Observer Pattern)
builder.Services.AddScoped<IBlogFacade>(provider =>
{
    var storage = provider.GetRequiredService<IBlogStorage>();
    var parser = provider.GetRequiredService<IMarkdownParser>();

    var facade = new BlogFacade(storage, parser);

    // Підписка на події
    facade.Subscribe(new CommentNotificationObserver());

    return facade;
});


// =======================
// 🔹 BUILD APP
// =======================

var app = builder.Build();

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

app.Run();