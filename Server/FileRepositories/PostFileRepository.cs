using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : GenericFileRepository<Post>, IPostRepository
{
    public PostFileRepository() : base("posts.json") { }

    public async Task<Post> AddAsync(Post post)
    {
        var posts = await LoadAsync();
        int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 0;
        post.Id = maxId + 1;
        posts.Add(post);
        await SaveAsync(posts);
        return post;
    }

    // В интерфейсе метод называется GetManyAsync и он НЕ async по типу
    public IQueryable<Post> GetManyAsync()
    {
        string json = File.ReadAllTextAsync(filePath).Result;
        var posts = System.Text.Json.JsonSerializer.Deserialize<List<Post>>(json)!;
        return posts.AsQueryable();
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        var posts = await LoadAsync();
        var entity = posts.FirstOrDefault(p => p.Id == id);
        if (entity is null) throw new KeyNotFoundException($"Post {id} not found");
        return entity;
    }

    public async Task UpdateAsync(Post post)
    {
        var posts = await LoadAsync();
        var idx = posts.FindIndex(p => p.Id == post.Id);
        if (idx != -1)
        {
            posts[idx] = post;
            await SaveAsync(posts);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var posts = await LoadAsync();
        var toRemove = posts.FirstOrDefault(p => p.Id == id);
        if (toRemove != null)
        {
            posts.Remove(toRemove);
            await SaveAsync(posts);
        }
    }
}