using System.Text.Json;

namespace FileRepositories;

public class GenericFileRepository<T>
{
    protected readonly string filePath;

    protected GenericFileRepository(string fileName)
    {
        filePath = fileName;
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    protected async Task<List<T>> LoadAsync()
    {
        string json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize < List<T>>(json)!;
    }

    protected async Task SaveAsync(List<T> entities)
    {
        string json = JsonSerializer.Serialize(entities);
        await File.WriteAllTextAsync(filePath, json);
    }
}