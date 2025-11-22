namespace Citas.Domain.Exceptions;

public class NotAuthorizedException : CitasException
{
  public NotAuthorizedException() : base(Resources.Exceptions.NotAuthorized) { }

  public NotAuthorizedException(string message) : base(message) { }

  public NotAuthorizedException(string message, Exception innerException) : base(message, innerException) { }
}
