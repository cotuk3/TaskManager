using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Exceptions.DomainExceptions;
public class DomainException : AppException
{
    public DomainException(string message, string errorCode, int statusCode)
        : base(message, errorCode, statusCode) { }
}
