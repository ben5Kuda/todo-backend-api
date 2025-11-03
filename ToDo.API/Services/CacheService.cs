using Microsoft.Extensions.Caching.Memory;
using ToDo.API.Models;

namespace ToDo.API.Services;

public class CacheService(IMemoryCache memoryCache): ICacheService
{
    const string CacheKey = "todos";
    public void SetCache(IEnumerable<TodoItem> todoItem)
    {
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(400))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3000))
            .SetPriority(CacheItemPriority.Normal);

        memoryCache.Set(CacheKey, todoItem, cacheOptions);
    }

    public IEnumerable<TodoItem> GetCache()
    {
        if (memoryCache.TryGetValue(CacheKey, out IEnumerable<TodoItem>? todoItems))
            return todoItems ?? [];

        var defaultTodos = GetDefaultToDos();
        SetCache(defaultTodos);
        return defaultTodos;
    }

    private List<TodoItem> GetDefaultToDos()
    {
        return
        [
            new TodoItem
            {
                Id = 1,
                Title = "Car wash",
                IsCompleted = false
            },

            new TodoItem
            {
                Id = 2,
                Title = "Grocery shopping",
                IsCompleted = false
            }
        ];
    }
}