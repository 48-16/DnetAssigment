using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : GenericFileRepository<Comment>, ICommentRepository
{
    public CommentFileRepository() : base("comments.json") { }

    public async Task<Comment> AddAsync(Comment comment)
    {
        var comments = await LoadAsync();
        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
        comment.Id = maxId + 1;
        comments.Add(comment);
        await SaveAsync(comments);
        return comment;
    }

    // В интерфейсе он называется GetManyAsync и НЕ async по сигнатуре
    public IQueryable<Comment> GetManyAsync()
    {
        string json = File.ReadAllTextAsync(filePath).Result;
        var comments = System.Text.Json.JsonSerializer.Deserialize<List<Comment>>(json)!;
        return comments.AsQueryable();
    }

    // В интерфейсе: GetSingleAsync (а не GetByIdAsync)
    public async Task<Comment> GetSingleAsync(int id)
    {
        var comments = await LoadAsync();
        // интерфейс требует Comment, поэтому используем ! (если не найден — вернётся null, но сигнатура такая)
        return comments.FirstOrDefault(c => c.Id == id)!;
    }

    public async Task UpdateAsync(Comment comment)
    {
        var comments = await LoadAsync();
        var idx = comments.FindIndex(c => c.Id == comment.Id);
        if (idx != -1)
        {
            comments[idx] = comment;
            await SaveAsync(comments);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var comments = await LoadAsync();
        var toRemove = comments.FirstOrDefault(c => c.Id == id);
        if (toRemove != null)
        {
            comments.Remove(toRemove);
            await SaveAsync(comments);
        }
    }
}