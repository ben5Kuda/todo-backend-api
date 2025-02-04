using ToDo.API.Models;

namespace ToDo.API.Services;

public interface ICacheService
{
    void SetCache(IEnumerable<TodoItem> todoItem);
    IEnumerable<TodoItem> GetCache();
}