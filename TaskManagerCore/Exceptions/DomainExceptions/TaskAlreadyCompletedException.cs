using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Exceptions.DomainExceptions;
public class TaskAlreadyCompletedException : DomainException
{
    public TaskAlreadyCompletedException(Guid taskId)
        : base($"Task '{taskId}' is already completed.", "TASK_ALREADY_COMPLETED", StatusCodes.Status400BadRequest) { }
}
