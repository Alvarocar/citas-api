namespace Citas.Domain.Exceptions;

public class NotFoundException : CitasException
{
  public NotFoundException() : base(Resources.Exceptions.NotFound) { }

  public NotFoundException(string message) : base(message) { }

  public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
