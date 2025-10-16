using System.ComponentModel.DataAnnotations;

namespace SkillManager.Application.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message)
        : base(message) { }
}
