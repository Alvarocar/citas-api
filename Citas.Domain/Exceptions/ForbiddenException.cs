namespace Citas.Domain.Exceptions;

public class ForbiddenException : CitasException
{
  public ForbiddenException() : base(Resources.Exceptions.Forbidden) { }
  public ForbiddenException(string message) : base(message) { }
  public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
}
