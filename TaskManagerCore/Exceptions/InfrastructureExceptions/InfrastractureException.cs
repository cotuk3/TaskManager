using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Exceptions.InfrastractureExceptions;
public class InfrastructureException : AppException
{
    protected InfrastructureException(string message, string errorCode, int statusCode)
            : base(message, errorCode, statusCode) { }
}
