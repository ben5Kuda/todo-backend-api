using Microsoft.AspNetCore.Mvc;
using ToDo.API.Models;
using ToDo.API.Services;

namespace ToDo.API.Endpoints;

[ApiController]
[Route("[controller]")]
public class TodosModule(ICacheService cacheService) : ControllerBase
{
    [HttpGet("GetTodos")]
    public async Task<IActionResult> Get()
    {
        await Task.Delay(5000);
        return await Task.FromResult<IActionResult>(Ok(cacheService.GetCache()));
    }

    [HttpPost("AddTodos")]
    public IActionResult Post(string title)
    {
        var currentToDos = cacheService.GetCache().ToList();
        if (currentToDos.Exists(x => x.Title == title))
            return BadRequest("Todo item already exists");

        var mostRecentTodo = currentToDos.MaxBy(x => x.Id);
        var todoItem = new TodoItem
        {
            Id = mostRecentTodo?.Id + 1 ?? 1,
            Title = title,
            IsCompleted = false
        };

        currentToDos.Add(todoItem);
        cacheService.SetCache(currentToDos);
        return Ok(todoItem);
    }

    [HttpPut("UpdateTodos/{id}")]
    public IActionResult Put(int id, bool isCompleted)
    {
        var currentToDos = cacheService.GetCache().ToList();

        var index = currentToDos.FindIndex(r => r.Id == id);

        if (index == -1)
            return BadRequest("Todo item does not exists");

        currentToDos[index].IsCompleted = isCompleted;

        cacheService.SetCache(currentToDos);
        return Ok();
    }

    [HttpDelete("DeleteTodos/{id}")]
    public IActionResult Delete(int id)
    {
        var currentToDos = cacheService.GetCache().ToList();

        var todoItem = currentToDos.Find(r => r.Id == id);

        if (todoItem is null)
            return BadRequest("Todo item does not exists");

        currentToDos.Remove(todoItem);

        cacheService.SetCache(currentToDos);
        return Ok();
    }
}