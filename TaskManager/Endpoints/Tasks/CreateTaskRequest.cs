namespace TaskManager.Api.Endpoints.Tasks;

public sealed class CreateTaskRequest
{
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public List<string> Labels { get; set; } = [];
    public int Priority { get; set; }
}
