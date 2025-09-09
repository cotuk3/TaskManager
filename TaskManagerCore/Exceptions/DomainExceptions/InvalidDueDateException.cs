using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Exceptions.DomainExceptions;
public class InvalidDueDateException : DomainException
{
    public InvalidDueDateException(DateTime date) : 
        base($"Invalid due date: {date:yyyy-MM-dd}", "INVALID_DUE_DATE", StatusCodes.Status400BadRequest) 
    { }
}