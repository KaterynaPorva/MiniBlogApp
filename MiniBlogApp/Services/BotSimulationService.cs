using Microsoft.Extensions.Hosting;
using MiniBlogApp.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniBlogApp.Services
{
    /// <summary>
    /// Фоновий сервіс, що імітує активність користувачів (ботів) у системі.
    /// Працює в окремому потоці та періодично додає лайки/коментарі.
    /// </summary>
    public class BotSimulationService : BackgroundService
    {
        private readonly IBlogStorage _storage;
        private readonly Random _random = new Random();

        // Отримуємо наше потокобезпечне сховище через Dependency Injection
        public BotSimulationService(IBlogStorage storage)
        {
            _storage = storage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Цикл працюватиме, поки додаток не буде зупинено
            while (!stoppingToken.IsCancellationRequested)
            {
                // Імітуємо паузу між діями ботів (від 2 до 5 секунд)
                await Task.Delay(_random.Next(2000, 5000), stoppingToken);

                var posts = _storage.GetAllPosts().ToList();

                if (posts.Any())
                {
                    // Вибираємо випадковий пост
                    var randomPost = posts[_random.Next(posts.Count)];

                    // Випадково вирішуємо: додати лайк чи коментар (50/50)
                    if (_random.NextDouble() > 0.5)
                    {
                        var botName = $"BotLiker_{_random.Next(1, 1000)}";
                        _storage.AddLike(randomPost.Id, botName);
                    }
                    else
                    {
                        var botName = $"BotCommenter_{_random.Next(1, 1000)}";
                        _storage.AddComment(randomPost.Id, botName, "Це автоматично згенерований коментар з фонового потоку!");
                    }
                }
                else
                {
                    // Якщо блог порожній, бот створює перший пост
                    var botPost = new Post
                    {
                        Title = "Привіт від Симулятора!",
                        Content = "Цей пост був автоматично створений фоновим потоком `BotSimulationService`.",
                        Author = "SystemBot"
                    };
                    _storage.AddPost(botPost);
                }
            }
        }
    }
}