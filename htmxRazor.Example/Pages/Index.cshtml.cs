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

    public IActionResult OnPostAdd(string title, TodoPriority priority, string? filter)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            _todoService.Add(title.Trim(), priority);
        }

        Response.HxTrigger("todoChanged");
        Todos = _todoService.GetFiltered(filter, null);
        return Partial("_TodoList", Todos);
    }

    public IActionResult OnPostToggle(int id, string? filter)
    {
        _todoService.Toggle(id);
        Response.HxTrigger("todoChanged");
        Todos = _todoService.GetFiltered(filter, null);
        return Partial("_TodoList", Todos);
    }

    public IActionResult OnPutUpdate(int id, string title, TodoPriority priority, string? filter)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            _todoService.Update(id, title.Trim(), priority);
        }

        Response.HxTrigger("todoChanged");
        Todos = _todoService.GetFiltered(filter, null);
        return Partial("_TodoList", Todos);
    }

    public IActionResult OnDeleteDelete(int id, string? filter)
    {
        _todoService.Delete(id);
        Response.HxTrigger("todoChanged");
        Todos = _todoService.GetFiltered(filter, null);
        return Partial("_TodoList", Todos);
    }

    public IActionResult OnDeleteClearCompleted(string? filter)
    {
        _todoService.ClearCompleted();
        Response.HxTrigger("todoChanged");
        Todos = _todoService.GetFiltered(filter, null);
        return Partial("_TodoList", Todos);
    }
}
