using MiniBlogApp.Models;

namespace MiniBlogApp.Builders
{
    public interface IPostBuilder
    {
        IPostBuilder SetTitle(string title);
        IPostBuilder SetContent(string content);

        IPostBuilder SetAuthor(string authorName);

        Post Build();
    }
}