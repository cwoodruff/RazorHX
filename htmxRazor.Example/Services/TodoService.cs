using htmxRazor.Example.Models;

namespace htmxRazor.Example.Services;

public class TodoService
{
    private static readonly List<TodoItem> _todos = new()
    {
        new() { Id = 1, Title = "Set up htmxRazor project", IsCompleted = true, Priority = TodoPriority.High, CreatedAt = DateTime.UtcNow.AddDays(-3), CompletedAt = DateTime.UtcNow.AddDays(-2) },
        new() { Id = 2, Title = "Build the To-Do app UI", IsCompleted = false, Priority = TodoPriority.High, CreatedAt = DateTime.UtcNow.AddDays(-2) },
        new() { Id = 3, Title = "Add dark mode support", IsCompleted = false, Priority = TodoPriority.Medium, CreatedAt = DateTime.UtcNow.AddDays(-1) },
        new() { Id = 4, Title = "Write documentation", IsCompleted = false, Priority = TodoPriority.Low, CreatedAt = DateTime.UtcNow },
    };

    private static int _nextId = 5;
    private static readonly object _lock = new();

    public List<TodoItem> GetAll() => _todos.OrderByDescending(t => t.CreatedAt).ToList();

    public List<TodoItem> GetFiltered(string? filter, string? search)
    {
        var query = _todos.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t => t.Title.Contains(search, StringComparison.OrdinalIgnoreCase));

        query = filter switch
        {
            "active" => query.Where(t => !t.IsCompleted),
            "completed" => query.Where(t => t.IsCompleted),
            _ => query
        };

        return query.OrderByDescending(t => t.CreatedAt).ToList();
    }

    public TodoItem? GetById(int id) => _todos.FirstOrDefault(t => t.Id == id);

    public TodoItem Add(string title, TodoPriority priority)
    {
        lock (_lock)
        {
            var todo = new TodoItem
            {
                Id = _nextId++,
                Title = title,
                Priority = priority,
                CreatedAt = DateTime.UtcNow
            };
            _todos.Add(todo);
            return todo;
        }
    }

    public TodoItem? Toggle(int id)
    {
        var todo = GetById(id);
        if (todo is null) return null;

        todo.IsCompleted = !todo.IsCompleted;
        todo.CompletedAt = todo.IsCompleted ? DateTime.UtcNow : null;
        return todo;
    }

    public TodoItem? Update(int id, string title, TodoPriority priority)
    {
        var todo = GetById(id);
        if (todo is null) return null;

        todo.Title = title;
        todo.Priority = priority;
        return todo;
    }

    public bool Delete(int id)
    {
        var todo = GetById(id);
        if (todo is null) return false;
        return _todos.Remove(todo);
    }

    public int ClearCompleted()
    {
        var count = _todos.RemoveAll(t => t.IsCompleted);
        return count;
    }

    public int TotalCount => _todos.Count;
    public int ActiveCount => _todos.Count(t => !t.IsCompleted);
    public int CompletedCount => _todos.Count(t => t.IsCompleted);
}
