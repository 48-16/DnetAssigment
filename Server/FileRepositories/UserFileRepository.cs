using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : GenericFileRepository<User>, IUserRepository
{
    public UserFileRepository() : base("users.json") { }

    public async Task<User> AddAsync(User user)
    {
        var users = await LoadAsync();
        int maxId = users.Count > 0 ? users.Max(u => u.Id) : 0;
        user.Id = maxId + 1;
        users.Add(user);
        await SaveAsync(users);
        return user;
    }

    // В интерфейсе метод называется GetManyAsync и он НЕ async по типу
    public IQueryable<User> GetManyAsync()
    {
        string json = File.ReadAllTextAsync(filePath).Result;
        var users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(json)!;
        return users.AsQueryable();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var users = await LoadAsync();
        var entity = users.FirstOrDefault(u => u.Id == id);
        if (entity is null) throw new KeyNotFoundException($"User {id} not found");
        return entity;
    }

    public async Task UpdateAsync(User user)
    {
        var users = await LoadAsync();
        var idx = users.FindIndex(u => u.Id == user.Id);
        if (idx != -1)
        {
            users[idx] = user;
            await SaveAsync(users);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var users = await LoadAsync();
        var toRemove = users.FirstOrDefault(u => u.Id == id);
        if (toRemove != null)
        {
            users.Remove(toRemove);
            await SaveAsync(users);
        }
    }
}