using System.Linq;
using System.Threading.Tasks;
using Xunit;
using MiniBlogApp.Models;
using MiniBlogApp.Services;

namespace MiniBlogApp.Tests.ServiceTests
{
    public class ConcurrentBlogStorageTests
    {
        [Fact]
        public void AddComment_UnderHeavyConcurrency_ShouldNotLoseData()
        {
            // Arrange: Створюємо наше потокобезпечне сховище та один базовий пост
            IBlogStorage storage = new ConcurrentBlogStorage();
            var post = new Post { Title = "Concurrency Test", Content = "Testing race conditions..." };
            var savedPost = storage.AddPost(post);

            int numberOfThreads = 100;
            int operationsPerThread = 100;
            int totalExpectedComments = numberOfThreads * operationsPerThread; // 10 000

            // Act: Використовуємо Parallel.For для імітації одночасної роботи 100 потоків
            Parallel.For(0, numberOfThreads, threadIndex =>
            {
                for (int i = 0; i < operationsPerThread; i++)
                {
                    // Усі потоки одночасно ломляться додати коментар до одного поста
                    storage.AddComment(savedPost.Id, $"User_{threadIndex}_{i}", "Multithreaded comment");
                }
            });

            // Assert: Перевіряємо, чи нічого не загубилося
            var updatedPost = storage.GetPostById(savedPost.Id);

            Assert.NotNull(updatedPost);
            Assert.NotNull(updatedPost.Comments);
            // Якщо блокування ReaderWriterLockSlim не працювало б, тут було б випадкове число < 10000
            Assert.Equal(totalExpectedComments, updatedPost.Comments.Count);
        }
    }
}