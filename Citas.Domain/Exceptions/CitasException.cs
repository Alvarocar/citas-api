namespace Citas.Domain.Exceptions;

public class CitasException : Exception
{

  public virtual string Type => GetType().Name;

  public CitasException(string? message) : base(message)
  {
  }
  public CitasException(string? message, Exception? innerException) : base(message, innerException)
  {
  }
}
