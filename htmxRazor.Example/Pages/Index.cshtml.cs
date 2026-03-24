using htmxRazor.Example.Models;
using htmxRazor.Example.Services;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace htmxRazor.Example.Pages;

public class IndexModel : PageModel
{
    private readonly TodoService _todoService;

    public IndexModel(TodoService todoService)
    {
        _todoService = todoService;
    }

    public List<TodoItem> Todos { get; set; } = [];
    public string? Filter { get; set; }
    public string? Search { get; set; }
    public int TotalCount => _todoService.TotalCount;
    public int ActiveCount => _todoService.ActiveCount;
    public int CompletedCount => _todoService.CompletedCount;
    public int ProgressPercent => TotalCount == 0 ? 0 : (int)Math.Round(100.0 * CompletedCount / TotalCount);
    public List<ActivityEntry> Activities => _todoService.GetRecentActivity();

    public void OnGet(string? filter, string? search)
    {
        Filter = filter;
        Search = search;
        Todos = _todoService.GetFiltered(filter, search);
    }

    public IActionResult OnGetTodoList(string? filter, string? search)
    {
        Filter = filter;
        Search = search;
        Todos = _todoService.GetFiltered(filter, search);
        return Partial("_TodoList", Todos);
    }

    public IActionResult OnGetStats()
    {
        return Partial("_Stats", this);
    }

    public IActionResult OnGetProgress()
    {
        return Partial("_Progress", this);
    }

    public IActionResult OnGetActivityLog()
    {
        return Partial("_ActivityLog", _todoService.GetRecentActivity());
    }

    public IActionResult OnPostAdd(string title, TodoPriority priority, string? filter)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            _todoService.Add(title.Trim(), priority);
            _todoService.LogActivity($"Added \"{title.Trim()}\"", "success", "plus");
            Response.HxToast($"Task \"{title.Trim()}\" added", "success");
        }

        Response.HxTrigger("todoChanged");
        Todos = _todoService.GetFiltered(filter, null);
        return Partial("_TodoList", Todos);
    }

    public IActionResult OnPostToggle(int id, string? filter)
    {
        var todo = _todoService.Toggle(id);
        if (todo is not null)
        {
            var action = todo.IsCompleted ? "completed" : "reopened";
            var variant = todo.IsCompleted ? "success" : "warning";
            var icon = todo.IsCompleted ? "check-circle" : "refresh";
            _todoService.LogActivity($"{char.ToUpper(action[0])}{action[1..]} \"{todo.Title}\"", variant, icon);
            Response.HxToast($"Task {action}", variant);
        }

        Response.HxTrigger("todoChanged");
        Todos = _todoService.GetFiltered(filter, null);
        return Partial("_TodoList", Todos);
    }

    public IActionResult OnPutUpdate(int id, string title, TodoPriority priority, string? filter)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            _todoService.Update(id, title.Trim(), priority);
            _todoService.LogActivity($"Updated \"{title.Trim()}\"", "brand", "edit");
            Response.HxToast("Task updated", "brand");
        }

        Response.HxTrigger("todoChanged");
        Todos = _todoService.GetFiltered(filter, null);
        return Partial("_TodoList", Todos);
    }

    public IActionResult OnDeleteDelete(int id, string? filter)
    {
        var todo = _todoService.GetById(id);
        _todoService.Delete(id);
        if (todo is not null)
        {
            _todoService.LogActivity($"Deleted \"{todo.Title}\"", "danger", "trash");
            Response.HxToast("Task deleted", "danger");
        }

        Response.HxTrigger("todoChanged");
        Todos = _todoService.GetFiltered(filter, null);
        return Partial("_TodoList", Todos);
    }

    public IActionResult OnDeleteClearCompleted(string? filter)
    {
        var count = _todoService.ClearCompleted();
        _todoService.LogActivity($"Cleared {count} completed task{(count != 1 ? "s" : "")}", "danger", "trash");
        Response.HxToast($"Cleared {count} completed task{(count != 1 ? "s" : "")}", "danger");

        Response.HxTrigger("todoChanged");
        Todos = _todoService.GetFiltered(filter, null);
        return Partial("_TodoList", Todos);
    }
}
