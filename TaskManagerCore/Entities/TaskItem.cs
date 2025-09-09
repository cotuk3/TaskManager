namespace TaskManager.Domain.Entities;
public record TaskItem
{
    public required Guid Id { get; set; }
    public required string? Description { get; set; } = string.Empty;
    public DueDate? DueDate { get; set; }
    public List<string>? Labels { get; set; } = [];
    public bool IsCompleted { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Priority? Priority { get; set; }

    public string UserId { get; set; }  // FK to Identity User
}
