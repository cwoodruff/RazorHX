using System.ComponentModel.DataAnnotations;

namespace htmxRazor.Example.Models;

public class TodoItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Task title is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
    public string Title { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public TodoPriority Priority { get; set; } = TodoPriority.Medium;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedAt { get; set; }
}

public enum TodoPriority
{
    Low,
    Medium,
    High
}

/// <summary>
/// Represents an entry in the activity log displayed as a timeline.
/// </summary>
public class ActivityEntry
{
    public string Message { get; set; } = string.Empty;
    public string Variant { get; set; } = "neutral";
    public string Icon { get; set; } = "info";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
