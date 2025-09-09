using TaskManager.Domain.Exceptions.DomainExceptions;

namespace TaskManager.Domain.Entities;
public class DueDate
{
    public DateTime? Value { get; }

    public DueDate(DateTime? value)
    {
        if(value.Value.Date < DateTime.UtcNow.Date)
            throw new InvalidDueDateException(value.Value);

        if(value.Value.Date > DateTime.UtcNow.AddYears(1).Date)
            throw new InvalidDueDateException(value.Value);

        Value = value;
    }
}
