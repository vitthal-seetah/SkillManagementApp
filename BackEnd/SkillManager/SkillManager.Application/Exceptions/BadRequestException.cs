using System.ComponentModel.DataAnnotations;

namespace SkillManager.Infrastructure.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message)
        : base(message) { }
}
