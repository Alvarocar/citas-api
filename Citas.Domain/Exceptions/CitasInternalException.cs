namespace Citas.Domain.Exceptions;

public class CitasInternalException : CitasException
{
  public CitasInternalException() : base(Resources.Exceptions.InternalError)
  {
  }
  public CitasInternalException(string? message) : base(message)
  {
  }

  public CitasInternalException(string? message, Exception? innerException) : base(message, innerException)
  {
  }
}
