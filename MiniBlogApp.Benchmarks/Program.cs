using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MiniBlogApp.Models;
using MiniBlogApp.Strategies;

namespace MiniBlogApp.Benchmarks
{
    // Цей атрибут додасть до результатів колонку з витратами оперативної пам'яті
    [MemoryDiagnoser]
    public class PostAnalyticsBenchmarks
    {
        private List<Post> _posts;
        private IPostAnalyticsStrategy _sequentialStrategy;
        private IPostAnalyticsStrategy _parallelStrategy;

        // Тестуємо алгоритми на двох обсягах даних: 10 тисяч та 100 тисяч постів
        [Params(10000, 100000)]
        public int PostCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _sequentialStrategy = new SequentialPostAnalyticsStrategy();
            _parallelStrategy = new ParallelPostAnalyticsStrategy();

            var random = new Random(42); // Фіксований seed для повторюваності результатів

            // Генеруємо масив фейкових постів для тестування
            _posts = Enumerable.Range(1, PostCount).Select(i => new Post
            {
                Id = i,
                Title = $"Post {i}",
                Content = new string('A', random.Next(50, 500)),
                Likes = Enumerable.Range(0, random.Next(0, 10)).Select(_ => new Like()).ToList(),
                // Зверніть увагу: ми створюємо базові коментарі, щоб навантажити алгоритм
                Comments = Enumerable.Range(0, random.Next(0, 5)).Select(_ => new Comment()).ToList()
            }).ToList();
        }

        // Встановлюємо послідовний алгоритм як базовий (Baseline)
        [Benchmark(Baseline = true)]
        public int Sequential_Analytics()
        {
            int likes = _sequentialStrategy.CalculateTotalLikes(_posts);
            int comments = _sequentialStrategy.CalculateTotalComments(_posts);
            int length = _sequentialStrategy.CalculateTotalContentLength(_posts);
            return likes + comments + length;
        }

        // Тестуємо паралельний алгоритм
        [Benchmark]
        public int Parallel_Analytics()
        {
            int likes = _parallelStrategy.CalculateTotalLikes(_posts);
            int comments = _parallelStrategy.CalculateTotalComments(_posts);
            int length = _parallelStrategy.CalculateTotalContentLength(_posts);
            return likes + comments + length;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Запуск процесу вимірювання
            var summary = BenchmarkRunner.Run<PostAnalyticsBenchmarks>();
        }
    }
}