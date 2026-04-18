using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using MiniBlogApp.Services;
using MiniBlogApp.Builders;
using MiniBlogApp.Facades;
using MiniBlogApp.Observers;
using MiniBlogApp.Proxies; // 1. ДОДАЛИ: Підключення папки з нашим Proxy
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

// ✅ 2. Proxy + Decorator (Збираємо "матрьошку" патернів)
builder.Services.AddSingleton<IBlogStorage>(provider =>
{
    // Крок 1: Отримуємо базове сховище
    var baseStorage = provider.GetRequiredService<BlogStorage>();

    // Крок 2: Обгортаємо його в КЕШУЮЧИЙ PROXY (Замісник)
    var cachedStorage = new CachedBlogStorageProxy(baseStorage);

    // Крок 3: Обгортаємо Proxy у ДЕКОРАТОР для логування
    var logger = provider.GetRequiredService<IActivityLogger>();
    return new LoggingBlogStorageDecorator(cachedStorage, logger);
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