namespace Citas.Domain.Exceptions;

public class ForbiddenException : CitasException
{
  public ForbiddenException() : base(Resources.Exceptions.Forbidden) { }

}
